using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Irony.Parsing;

namespace Irony.Samples.SQL
{
    // Loosely based on SQL89 grammar from Gold parser. Supports some extra TSQL constructs.

    [Language("SQL", "89", "SQL 89 grammar")]
    public class SqlGrammar : Grammar
    {
        public SqlGrammar()
            : base(false)
        { //SQL is case insensitive
            //Terminals
            var comment = new CommentTerminal("comment", "/*", "*/");
            var lineComment = new CommentTerminal("line_comment", "--", "\n", "\r\n");
            NonGrammarTerminals.Add(comment);
            NonGrammarTerminals.Add(lineComment);
            var number = TerminalFactory.CreateCSharpNumber("number");
            var string_literal = new StringLiteral("string", "'", StringOptions.AllowsDoubledQuote);
            string_literal.AddStartEnd("N'", "'", StringOptions.AllowsDoubledQuote | StringOptions.AllowsLineBreak);
            var Id_simple = TerminalFactory.CreateSqlExtIdentifier(this, "id_simple"); //covers normal identifiers (abc) and quoted id's ([abc d], "abc d")
            Id_simple.AddPrefix("@@", IdOptions.NameIncludesPrefix);
            Id_simple.AddPrefix("@", IdOptions.NameIncludesPrefix);

            var comma = ToTerm(",");
            var dot = ToTerm(".");
            var equals = ToTerm("=");
            var CREATE = ToTerm("CREATE");
            var NULL = ToTerm("NULL");
            var NOT = ToTerm("NOT");
            var UNIQUE = ToTerm("UNIQUE");
            var WITH = ToTerm("WITH");
            var TABLE = ToTerm("TABLE");
            var VIEW = ToTerm("VIEW");
            var DATABASE = ToTerm("DATABASE");
            var ALTER = ToTerm("ALTER");
            var ADD = ToTerm("ADD");
            var COLUMN = ToTerm("COLUMN");
            var DROP = ToTerm("DROP");
            var CONSTRAINT = ToTerm("CONSTRAINT");
            var INDEX = ToTerm("INDEX");
            var CLUSTERED = ToTerm("CLUSTERED");
            var NONCLUSTERED = ToTerm("NONCLUSTERED");
            var ON = ToTerm("ON");
            var KEY = ToTerm("KEY");
            var PRIMARY = ToTerm("PRIMARY");
            var INSERT = ToTerm("INSERT");
            var INTO = ToTerm("INTO");
            var UPDATE = ToTerm("UPDATE");
            var SET = ToTerm("SET");
            var VALUES = ToTerm("VALUES");
            var DELETE = ToTerm("DELETE");
            var SELECT = ToTerm("SELECT");
            var FROM = ToTerm("FROM");
            var AS = ToTerm("AS");
            var COUNT = ToTerm("COUNT");
            var JOIN = ToTerm("JOIN");
            var BY = ToTerm("BY");
            var DEFAULT = ToTerm("DEFAULT");
            var CHECK = ToTerm("CHECK");
            var REPLICATION = ToTerm("REPLICATION");
            var FOR = ToTerm("FOR");
            var COLLATE = ToTerm("COLLATE");
            var IDENTITY = ToTerm("IDENTITY");
            var TEXTIMAGE_ON = ToTerm("TEXTIMAGE_ON");
            var IF = ToTerm("IF");
            var ELSE = ToTerm("ELSE");
            var BEGIN = ToTerm("BEGIN");
            var END = ToTerm("END");
            var GO = ToTerm("GO");
            var PRINT = ToTerm("PRINT");
            var IS = ToTerm("IS");
            var USE = ToTerm("USE");
            var EXEC = ToTerm("EXEC");
            var NOCHECK = ToTerm("NOCHECK");
            var CASCADE = ToTerm("CASCADE");
            var TYPE = ToTerm("TYPE");

            //Non-terminals
            var Id = new NonTerminal("Id");
            var IdWithAliasOpt = new NonTerminal("IdWithAliasOpt");
            var stmt = new NonTerminal("stmt");
            var createTableStmt = new NonTerminal("createTableStmt");
            var createIndexStmt = new NonTerminal("createIndexStmt");
            var createViewStmt = new NonTerminal("createViewStmt");
            var createTypeStmt = new NonTerminal("createTypeStmt");
            var primaryKeyOpt = new NonTerminal("primaryKeyOpt");
            var alterStmt = new NonTerminal("alterStmt");
            var dropTableStmt = new NonTerminal("dropTableStmt");
            var dropIndexStmt = new NonTerminal("dropIndexStmt");
            var dropViewStmt = new NonTerminal("dropViewStmt");
            var selectStmt = new NonTerminal("selectStmt");
            var insertStmt = new NonTerminal("insertStmt");
            var updateStmt = new NonTerminal("updateStmt");
            var deleteStmt = new NonTerminal("deleteStmt");
            var ifStmt = new NonTerminal("ifStmt");
            var elseStmt = new NonTerminal("elseStmt");
            var fieldDef = new NonTerminal("fieldDef");
            var fieldDefList = new NonTerminal("fieldDefList");
            var nullSpecOpt = new NonTerminal("nullSpecOpt");
            var typeName = new NonTerminal("typeName");
            var typeSpec = new NonTerminal("typeSpec");
            var constraintListOpt = new NonTerminal("constraintListOpt");
            var constraintDef = new NonTerminal("constraintDef");
            var indexContraintDef = new NonTerminal("indexContraintDef");
            var constraintType = new NonTerminal("constraintTypeOpt");
            var defaultValueOpt = new NonTerminal("defaultValueOpt");
            var idlist = new NonTerminal("idlist");
            var idlistForSelect = new NonTerminal("idlistForSelect");
            var sprocParamList = new NonTerminal("sprocParamList");
            var idlistParOpt = new NonTerminal("idlistPar");
            var orderList = new NonTerminal("orderList");
            var orderMember = new NonTerminal("orderMember");
            var orderDirOpt = new NonTerminal("orderDirOpt");
            var defaultValueParams = new NonTerminal("defaultValueParams");
            var indexTypeOpt = new NonTerminal("indexTypeOpt");
            var indexTypeList = new NonTerminal("indexTypeList");
            var withClauseOpt = new NonTerminal("withClauseOpt");
            var alterCmdOpt = new NonTerminal("alterCmdOpt");
            var insertData = new NonTerminal("insertData");
            var intoOpt = new NonTerminal("intoOpt");
            var assignList = new NonTerminal("assignList");
            var whereClauseOpt = new NonTerminal("whereClauseOpt");
            var andClauseOpt = new NonTerminal("andClauseOpt");
            var assignment = new NonTerminal("assignment");
            var expression = new NonTerminal("expression");
            var exprList = new NonTerminal("exprList");
            var selRestrOpt = new NonTerminal("selRestrOpt");
            var selList = new NonTerminal("selList");
            var intoClauseOpt = new NonTerminal("intoClauseOpt");
            var fromClauseOpt = new NonTerminal("fromClauseOpt");
            var groupClauseOpt = new NonTerminal("groupClauseOpt");
            var havingClauseOpt = new NonTerminal("havingClauseOpt");
            var orderClauseOpt = new NonTerminal("orderClauseOpt");
            var columnItemList = new NonTerminal("columnItemList");
            var columnItem = new NonTerminal("columnItem");
            var columnSource = new NonTerminal("columnSource");
            var asOpt = new NonTerminal("asOpt");
            var aliasOpt = new NonTerminal("aliasOpt");
            var aggregate = new NonTerminal("aggregate");
            var aggregateArg = new NonTerminal("aggregateArg");
            var aggregateName = new NonTerminal("aggregateName");
            var tuple = new NonTerminal("tuple");
            var joinChainOpt = new NonTerminal("joinChainOpt");
            var joinKindOpt = new NonTerminal("joinKindOpt");
            var term = new NonTerminal("term");
            var unExpr = new NonTerminal("unExpr");
            var unOp = new NonTerminal("unOp");
            var binExpr = new NonTerminal("binExpr");
            var binOp = new NonTerminal("binOp");
            var betweenExpr = new NonTerminal("betweenExpr");
            var inExpr = new NonTerminal("inExpr");
            var parSelectStmt = new NonTerminal("parSelectStmt");
            var notOpt = new NonTerminal("notOpt");
            var funCall = new NonTerminal("funCall");
            var stmtLine = new NonTerminal("stmtLine");
            var semiOpt = new NonTerminal("semiOpt");
            var stmtList = new NonTerminal("stmtList");
            var funArgsOpt = new NonTerminal("funArgsOpt");
            var inStmt = new NonTerminal("inStmt");
            var settingList = new NonTerminal("settingList");
            var settingListItem = new NonTerminal("settingListItem");
            var onOpt = new NonTerminal("onOpt");
            var textImageOnOpt = new NonTerminal("textImageOnOpt");
            var defaultValueParamsList = new NonTerminal("defaultValueParamsList");
            var typeNameParamsList = new NonTerminal("typeNameParamsList");
            var notForReplOpt = new NonTerminal("notForReplOpt");
            var collateOpt = new NonTerminal("collateOpt");
            var columnDef = new NonTerminal("columnDef");
            var identityOpt = new NonTerminal("identityOpt");
            var referencesOpt = new NonTerminal("referencesOpt");
            var fieldDefLists = new NonTerminal("fieldDefLists");
            var onListOpt = new NonTerminal("onListOpt");
            var printStmt = new NonTerminal("printStmt");
            var beginEndStmt = new NonTerminal("beginEndStmt");
            var beginEndStmtList = new NonTerminal("beginEndStmtList");
            var isNullOpt = new NonTerminal("isNullOpt");
            var setStmtOpt = new NonTerminal("setStmtOpt");
            var useStmt = new NonTerminal("useStmt");
            var forOpt = new NonTerminal("forOpt");
            var execStmt = new NonTerminal("execStmt");
            var sprocParam = new NonTerminal("sprocParam");
            var cascadeOpt = new NonTerminal("cascadeOpt");
            var cascadeListOpt = new NonTerminal("cascadeListOpt");

            //BNF Rules
            this.Root = stmtList;
            stmtLine.Rule = stmt + semiOpt;
            semiOpt.Rule = Empty | ";";
            stmtList.Rule = MakePlusRule(stmtList, stmtLine);
            setStmtOpt.Rule = Empty | SET + Id + Id;
            useStmt.Rule = USE + Id;
            execStmt.Rule = EXEC + Id + sprocParamList;

            //ID
            Id.Rule = MakePlusRule(Id, dot, Id_simple);
            IdWithAliasOpt.Rule = Id | Id + Id | Id + AS + Id;

            stmt.Rule = createTableStmt | createIndexStmt | createViewStmt | createTypeStmt
                      | alterStmt | dropTableStmt | dropIndexStmt | dropViewStmt
                      | selectStmt | insertStmt | updateStmt | deleteStmt
                      | GO | ifStmt | elseStmt | beginEndStmt | printStmt
                      | execStmt | setStmtOpt | useStmt | ";";

            onOpt.Rule = Empty | ON + Id;
            textImageOnOpt.Rule = Empty | TEXTIMAGE_ON + Id;
            forOpt.Rule = Empty | FOR + Id;
            onListOpt.Rule = MakePlusRule(onListOpt, onOpt);

            fieldDefLists.Rule = MakePlusRule(fieldDefLists, fieldDefList);

            printStmt.Rule = PRINT + string_literal;

            beginEndStmtList.Rule = MakePlusRule(beginEndStmtList, stmt);
            beginEndStmt.Rule = BEGIN + beginEndStmtList + END;

            isNullOpt.Rule = Empty | IS + NULL;

            // If
            ifStmt.Rule = IF + notOpt + funCall + isNullOpt;
            elseStmt.Rule = ELSE;

            createViewStmt.Rule = CREATE + VIEW + Id + AS + selectStmt;
            createTypeStmt.Rule = CREATE + TYPE + Id + FROM + Id;

            //Create table
            createTableStmt.Rule = CREATE + TABLE + Id + "(" + fieldDefLists + ")" + (onOpt | withClauseOpt) + textImageOnOpt;
            fieldDefList.Rule = MakeListRule(fieldDefList, comma, fieldDef, TermListOptions.AllowTrailingDelimiter | TermListOptions.PlusList);
            fieldDef.Rule = columnDef | constraintListOpt;
            columnDef.Rule = Id + typeName + collateOpt + primaryKeyOpt + nullSpecOpt + referencesOpt + defaultValueOpt + withClauseOpt
                | Id + typeName + collateOpt + primaryKeyOpt + nullSpecOpt + constraintListOpt + withClauseOpt
                | Id + typeName + collateOpt + primaryKeyOpt + notForReplOpt + nullSpecOpt + defaultValueOpt + withClauseOpt
                | Id + typeName + collateOpt + primaryKeyOpt + notForReplOpt + nullSpecOpt + constraintListOpt + withClauseOpt
                | primaryKeyOpt + indexTypeOpt + idlistParOpt + withClauseOpt
                | term;
            referencesOpt.Rule = Empty | "References" + Id + idlistParOpt;
            notForReplOpt.Rule = Empty | (NOT + FOR + REPLICATION);
            nullSpecOpt.Rule = Empty | (NOT + FOR + REPLICATION) | NULL | NOT + NULL | NOT + NULL + typeName | NULL + typeName;
            collateOpt.Rule = Empty | COLLATE + Id_simple;
            identityOpt.Rule = Empty | IDENTITY;
            typeNameParamsList.Rule = MakePlusRule(typeNameParamsList, comma, term);
            typeName.Rule = Id_simple | Id_simple + "(" + typeNameParamsList + ")";
            constraintDef.Rule = CONSTRAINT + Id + constraintType + onListOpt;
            indexContraintDef.Rule = constraintType + onListOpt;
            constraintListOpt.Rule = MakeStarRule(constraintListOpt, constraintDef);
            constraintType.Rule = defaultValueOpt + withClauseOpt
                | primaryKeyOpt + indexTypeList + idlistParOpt + withClauseOpt
                | CHECK + "(" + expression + ")" + withClauseOpt
                | NOT + NULL + idlistParOpt + withClauseOpt
                | "Foreign" + KEY + idlistParOpt + referencesOpt + notForReplOpt + withClauseOpt
                | "INCLUDE" + idlistParOpt + withClauseOpt + onOpt;
            idlistParOpt.Rule = Empty | "(" + orderList + ")";
            idlist.Rule = MakePlusRule(idlist, comma, Id);
            idlistForSelect.Rule = MakePlusRule(idlist, comma, IdWithAliasOpt);
            defaultValueParamsList.Rule = MakePlusRule(defaultValueParamsList, comma, term);
            sprocParam.Rule = Id + "=" + term;
            sprocParamList.Rule = MakePlusRule(sprocParamList, comma, sprocParam);
            defaultValueOpt.Rule = Empty | (DEFAULT + defaultValueParams);
            defaultValueParams.Rule = term | "(" + term + ")";

            //Create Index
            primaryKeyOpt.Rule = Empty | PRIMARY + KEY | typeName;
            createIndexStmt.Rule = CREATE + indexTypeList + INDEX + Id + onOpt + "(" + orderList + ")" + withClauseOpt + onOpt
                                 | CREATE + indexTypeList + INDEX + Id + onOpt + "(" + orderList + ")" + constraintType + whereClauseOpt + onOpt;
            orderList.Rule = MakePlusRule(orderList, comma, orderMember);
            orderMember.Rule = Id + orderDirOpt;
            orderDirOpt.Rule = Empty | "ASC" | "DESC";
            indexTypeOpt.Rule = Empty | UNIQUE | CLUSTERED | NONCLUSTERED;
            indexTypeList.Rule = MakeStarRule(indexTypeList, indexTypeOpt);
            settingList.Rule = MakePlusRule(settingList, comma, settingListItem);
            settingListItem.Rule = Id + equals + term;
            withClauseOpt.Rule = Empty | (WITH + PRIMARY | WITH + "Disallow" + NULL | WITH + "Ignore" + NULL | WITH + "(" + settingList + ")" + onOpt + textImageOnOpt);

            cascadeOpt.Rule = Empty | (ON + (UPDATE | DELETE) + CASCADE);
            cascadeListOpt.Rule = MakePlusRule(cascadeListOpt, cascadeOpt);

            //Alter 
            alterStmt.Rule = ALTER + (TABLE | DATABASE) + Id + setStmtOpt + alterCmdOpt;
            alterCmdOpt.Rule = Empty | ADD + COLUMN + fieldDefList + constraintDef
                          | CHECK + CONSTRAINT + Id
                          | WITH + (CHECK | NOCHECK) + ADD + CONSTRAINT + Id + constraintType + cascadeListOpt
                          | ADD + constraintDef + forOpt
                          | DROP + COLUMN + Id
                          | DROP + CONSTRAINT + Id;

            //Drop stmts
            dropTableStmt.Rule = DROP + TABLE + Id;
            dropIndexStmt.Rule = DROP + INDEX + Id + ON + Id;
            dropViewStmt.Rule = DROP + VIEW + Id;

            //Insert stmt
            insertStmt.Rule = INSERT + intoOpt + Id + idlistParOpt + insertData;
            insertData.Rule = selectStmt | VALUES + "(" + exprList + ")";
            intoOpt.Rule = Empty | INTO; //Into is optional in MSSQL

            //Update stmt
            updateStmt.Rule = UPDATE + Id + SET + assignList + whereClauseOpt + andClauseOpt;
            assignList.Rule = MakePlusRule(assignList, comma, assignment);
            assignment.Rule = Id + "=" + expression;

            //Delete stmt
            deleteStmt.Rule = DELETE + FROM + Id + whereClauseOpt + andClauseOpt;

            //Select stmt
            selectStmt.Rule = SELECT + selRestrOpt + selList + intoClauseOpt + fromClauseOpt + joinChainOpt + whereClauseOpt + andClauseOpt +
                              groupClauseOpt + havingClauseOpt + orderClauseOpt;
            selRestrOpt.Rule = Empty | "ALL" | "DISTINCT";
            selList.Rule = columnItemList | "*";
            columnItemList.Rule = MakePlusRule(columnItemList, comma, columnItem);
            columnItem.Rule = columnSource + aliasOpt;
            aliasOpt.Rule = Empty | asOpt + Id;
            asOpt.Rule = Empty | AS;
            columnSource.Rule = aggregate | Id;
            aggregate.Rule = aggregateName + "(" + aggregateArg + ")";
            aggregateArg.Rule = expression | "*";
            aggregateName.Rule = COUNT | "Avg" | "Min" | "Max" | "StDev" | "StDevP" | "Sum" | "Var" | "VarP";
            intoClauseOpt.Rule = Empty | INTO + Id;
            fromClauseOpt.Rule = Empty | FROM + idlistForSelect + joinChainOpt;
            joinChainOpt.Rule = Empty | joinKindOpt + JOIN + idlist + ON + Id + "=" + Id;
            joinKindOpt.Rule = Empty | "INNER" | "LEFT" | "RIGHT";
            whereClauseOpt.Rule = Empty | "WHERE" + expression;
            andClauseOpt.Rule = Empty | "AND" + expression;
            groupClauseOpt.Rule = Empty | "GROUP" + BY + idlist;
            havingClauseOpt.Rule = Empty | "HAVING" + expression;
            orderClauseOpt.Rule = Empty | "ORDER" + BY + orderList;

            //Expression
            exprList.Rule = MakePlusRule(exprList, comma, expression);
            expression.Rule = term | unExpr | binExpr;// | betweenExpr; //-- BETWEEN doesn't work - yet; brings a few parsing conflicts 
            term.Rule = Id | string_literal | number | funCall | tuple | parSelectStmt;// | inStmt;
            tuple.Rule = "(" + exprList + ")";
            parSelectStmt.Rule = "(" + selectStmt + ")";
            unExpr.Rule = unOp + term;
            unOp.Rule = NOT | "+" | "-" | "~";
            binExpr.Rule = expression + binOp + expression;
            binOp.Rule = ToTerm("+") | "-" | "*" | "/" | "%" //arithmetic
                       | "&" | "|" | "^"                     //bit
                       | "=" | ">" | "<" | ">=" | "<=" | "<>" | "!=" | "!<" | "!>"
                       | "AND" | "OR" | "LIKE" | NOT + "LIKE" | IS | "IN" | NOT + "IN";
            betweenExpr.Rule = expression + notOpt + "BETWEEN" + expression + "AND" + expression;
            notOpt.Rule = Empty | NOT;
            //funCall covers some psedo-operators and special forms like ANY(...), SOME(...), ALL(...), EXISTS(...), IN(...)
            funCall.Rule = Id + "(" + funArgsOpt + ")";
            funArgsOpt.Rule = Empty | selectStmt | exprList;
            inStmt.Rule = expression + "IN" + "(" + exprList + ")";

            //Operators
            RegisterOperators(10, "*", "/", "%");
            RegisterOperators(9, "+", "-");
            RegisterOperators(8, "=", ">", "<", ">=", "<=", "<>", "!=", "!<", "!>", "LIKE", "IN");
            RegisterOperators(7, "^", "&", "|");
            RegisterOperators(6, NOT);
            RegisterOperators(5, "AND");
            RegisterOperators(4, "OR");

            MarkPunctuation(",", "(", ")");
            MarkPunctuation(asOpt, semiOpt);
            //Note: we cannot declare binOp as transient because it includes operators "NOT LIKE", "NOT IN" consisting of two tokens. 
            // Transient non-terminals cannot have more than one non-punctuation child nodes.
            // Instead, we set flag InheritPrecedence on binOp , so that it inherits precedence value from it's children, and this precedence is used
            // in conflict resolution when binOp node is sitting on the stack
            base.MarkTransient(stmt, term, asOpt, aliasOpt, stmtLine, expression, unOp, tuple);
            binOp.SetFlag(TermFlags.InheritPrecedence);

        }//constructor
    }//class
}//namespace
