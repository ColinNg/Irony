#region License
/* **********************************************************************************
 * Copyright (c) Roman Ivantsov
 * This source code is subject to terms and conditions of the MIT License
 * for Irony. A copy of the license can be found in the License.txt file
 * at the root of this distribution. 
 * By using this source code in any fashion, you are agreeing to be bound by the terms of the 
 * MIT License.
 * You must not remove this notice from this software.
 * **********************************************************************************/
#endregion

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;

namespace Irony.Parsing {

  //Scanner class. The Scanner's function is to transform a stream of characters into aggregates/words or lexemes, 
  // like identifier, number, literal, etc. 

  public class Scanner  {
    #region Properties and Fields: Data, _source
    public readonly ScannerData Data;
    public readonly Parser Parser;
    Grammar _grammar;
    //buffered tokens can come from expanding a multi-token, when Terminal.TryMatch() returns several tokens packed into one token

    private ParsingContext Context {
      get { return Parser.Context; }
    }
    #endregion

    public Scanner(Parser parser) {
      Parser = parser; 
      Data = parser.Language.ScannerData;
      _grammar = parser.Language.Grammar;
      Context.SourceStream = new SourceStream(this.Data, Context.TabWidth);
      //create token streams
      var tokenStream = GetUnfilteredTokens();
      //chain all token filters
      Context.TokenFilters.Clear();
      _grammar.CreateTokenFilters(Data.Language, Context.TokenFilters);
      foreach (TokenFilter filter in Context.TokenFilters) {
        tokenStream = filter.BeginFiltering(Context, tokenStream);
      }
      Context.FilteredTokens = tokenStream.GetEnumerator();
    }

    internal void OnStatusChanged(ParserStatus oldStatus) {
    }

    public Token GetToken() {
      //get new token from pipeline
      if (!Context.FilteredTokens.MoveNext()) return null;
      var token = Context.FilteredTokens.Current;
      if (Context.Status == ParserStatus.Previewing)
        Context.PreviewTokens.Push(token);
      else 
        Context.CurrentParseTree.Tokens.Add(token);
      return token;
    }

    //This is iterator method, so it returns immediately when called directly
    // returns unfiltered, "raw" token stream
    private IEnumerable<Token> GetUnfilteredTokens() {
      //We don't do "while(!_source.EOF())... because on EOF() we need to continue and produce EOF token 
      while (true) {  
        Context.CurrentTerminals.Clear(); 
        Context.PreviousToken = Context.CurrentToken;
        Context.CurrentToken = null; 
        NextToken();
        Context.OnTokenCreated();
        yield return Context.CurrentToken;
        //Don't yield break, continue returning EOF
      }//while
    }// method

    #region Scanning tokens
    private void NextToken() {
      //1. Check if there are buffered tokens
      if(Context.BufferedTokens.Count > 0) {
        Context.CurrentToken = Context.BufferedTokens.Pop();
        return; 
      }
      //2. Skip whitespace. We don't need to check for EOF: at EOF we start getting 0-char, so we'll get out automatically
      while (_grammar.WhitespaceChars.IndexOf(Context.SourceStream.PreviewChar) >= 0)
        Context.SourceStream.PreviewPosition++;
      //3. That's the token start, calc location (line and column)
      Context.SourceStream.MoveLocationToPreviewPosition();
      //4. Check for EOF
      if (Context.SourceStream.EOF()) {
        CreateFinalTokensInBuffer(); //puts Eof and optionally final NewLine tokens into buffer
        Context.CurrentToken = Context.BufferedTokens.Pop();
        return; 
      }
      //5. Actually scan the source text and construct a new token
      ScanToken(); 
    }//method

    //Scans the source text and constructs a new token
    private void ScanToken() {
      //Find matching terminal
      // First, try terminals with explicit "first-char" prefixes, selected by current char in source
      ComputeCurrentTerminals();
      //If we have more than one candidate; let grammar method select
      if (Context.CurrentTerminals.Count > 1)
        _grammar.OnScannerSelectTerminal(Context);
 
      MatchTerminals();
      //If we don't have a token from terminals, try Grammar's method
      if (Context.CurrentToken == null)
        Context.CurrentToken = _grammar.TryMatch(Context, Context.SourceStream);
      if (Context.CurrentToken is MultiToken)
        UnpackMultiToken();
      var token = Context.CurrentToken;
      //If we have normal token then return it
      if (token != null && !token.IsError()) {
        //set position to point after the result token
        Context.SourceStream.PreviewPosition = Context.SourceStream.Location.Position + token.Length;
        Context.SourceStream.MoveLocationToPreviewPosition();
        return;
      }
      //we have an error: either error token or no token at all
      if (token == null)  //if no token then create error token
        Context.CurrentToken = Context.SourceStream.CreateErrorToken(Resources.ErrInvalidChar, Context.SourceStream.PreviewChar);
      Recover();
      
    }

    //If token is MultiToken then push all its child tokens into _bufferdTokens and return the first token in buffer
    private void UnpackMultiToken() {
      var mtoken = Context.CurrentToken as MultiToken;
      if (mtoken == null) return; 
      for (int i = mtoken.ChildTokens.Count-1; i >= 0; i--)
        Context.BufferedTokens.Push(mtoken.ChildTokens[i]);
      Context.CurrentToken = Context.BufferedTokens.Pop();
    }
    
    //creates final NewLine token (if necessary) and EOF token and puts them into _bufferedTokens
    private void CreateFinalTokensInBuffer() {
      //check if we need extra newline before EOF
      bool previousIsNewLine = Context.PreviousToken != null && Context.PreviousToken.Terminal == _grammar.NewLine;
      var eofToken = new Token(_grammar.Eof, Context.SourceStream.Location, string.Empty, _grammar.Eof.Name);
      Context.BufferedTokens.Push(eofToken); //put it into buffer
      if (_grammar.FlagIsSet(LanguageFlags.NewLineBeforeEOF) && !previousIsNewLine) {
        var newLineToken = new Token(_grammar.NewLine, Context.SourceStream.Location, Environment.NewLine, null);
        Context.BufferedTokens.Push(newLineToken); 
      }//if
    }

    private void ComputeCurrentTerminals() {
      TerminalList termsForCurrentChar;
      if(!Data.TerminalsLookup.TryGetValue(Context.SourceStream.PreviewChar, out termsForCurrentChar)) 
        return;
      //if we are recovering, previewing or there's no parser state, then return list as is
      // Also return list as is if there are token filters
      // Token filters inject/remove tokens from the stream, so the tokens parser is expecting might be different from
      // scanner can scan
      if(Context.Status == ParserStatus.Recovering || Context.Status == ParserStatus.Previewing 
          || Context.CurrentParserState == null || Context.TokenFilters.Count > 0) {
        Context.CurrentTerminals.AddRange(termsForCurrentChar);
        return; 
      }
      // Try filtering terms by checking with parser which terms it expects; 
      var parserState = Context.CurrentParserState;
      foreach(var term in termsForCurrentChar) {
        if(parserState.ExpectedTerms.Contains(term) || _grammar.NonGrammarTerminals.Contains(term))
          Context.CurrentTerminals.Add(term);
      }

    }//method

    private void MatchTerminals() {
      Token priorToken = null;
      foreach (Terminal term in Context.CurrentTerminals) {
        // Check if the term has lower priority that result token we already have; 
        //  if term.Priority is lower then we don't need to check anymore, higher priority wins
        // Note that terminals in the list are sorted in descending priority order
        if (priorToken  != null && priorToken.Terminal.Priority > term.Priority)
          return;
        //Reset source position and try to match
        Context.SourceStream.PreviewPosition = Context.SourceStream.Location.Position;
        var token = term.TryMatch(Context, Context.SourceStream);
        if (token == null) continue; 
        //skip it if it is shorter than previous token
        if (priorToken != null && !priorToken.IsError() && (token.Length < priorToken.Length))
          continue; 
        Context.CurrentToken = token; //now it becomes current token
        term.InvokeValidateToken(Context); //validate it
        if (Context.CurrentToken == null) continue; //if it was rejected
        priorToken = Context.CurrentToken;
      }
    }//method

    #endregion

    #region VS Integration methods
    //Use this method for VS integration; VS language package requires scanner that returns tokens one-by-one. 
    // Start and End positions required by this scanner may be derived from Token : 
    //   start=token.Location.Position; end=start + token.Length;
    public Token VsReadToken(ref int state) {
      Context.VsLineScanState.Value = state;
      if (Context.SourceStream.EOF()) return null;
      if (state == 0)
        NextToken();
      else {
        Terminal term = Data.MultilineTerminals[Context.VsLineScanState.TerminalIndex - 1];
        Context.CurrentToken = term.TryMatch(Context, Context.SourceStream); 
      }
      //set state value from context
      state = Context.VsLineScanState.Value;
      if (Context.CurrentToken != null && Context.CurrentToken.Terminal == _grammar.Eof)
        return null; 
      return Context.CurrentToken;
    }
    public void VsSetSource(string text, int offset) {
      Context.SourceStream.SetText(text, offset, true);
    }
    #endregion

    #region Error recovery
    private bool Recover() {
      Context.SourceStream.PreviewPosition++;
      var wsd = Data.Language.GrammarData.WhitespaceAndDelimiters;
      while (!Context.SourceStream.EOF()) {
        if(wsd.IndexOf(Context.SourceStream.PreviewChar) >= 0) {
          Context.SourceStream.MoveLocationToPreviewPosition();
          return true;
        }
        Context.SourceStream.PreviewPosition++;
      }
      return false; 
    }
    #endregion 

    #region TokenPreview
    //Preview mode allows custom code in grammar to help parser decide on appropriate action in case of conflict
    // Preview process is simply searching for particular tokens in "preview set", and finding out which of the 
    // tokens will come first.
    // In preview mode, tokens returned by FetchToken are collected in _previewTokens list; after finishing preview
    //  the scanner "rolls back" to original position - either by directly restoring the position, or moving the preview
    //  tokens into _bufferedTokens list, so that they will read again by parser in normal mode.
    // See c# grammar sample for an example of using preview methods
    SourceLocation _previewStartLocation;

    //Switches Scanner into preview mode
    public void BeginPreview() {
      Context.Status = ParserStatus.Previewing;
      _previewStartLocation = Context.SourceStream.Location;
      Context.PreviewTokens.Clear();
    }

    //Ends preview mode
    public void EndPreview(bool keepPreviewTokens) {
      if (keepPreviewTokens) {
        //insert previewed tokens into buffered list, so we don't recreate them again
        while (Context.PreviewTokens.Count > 0)
          Context.BufferedTokens.Push(Context.PreviewTokens.Pop()); 
      }  else
        Context.SetSourceLocation(_previewStartLocation);
      Context.PreviewTokens.Clear();
      Context.Status = ParserStatus.Parsing;
    }
    #endregion


  }//class

}//namespace