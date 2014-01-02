using Irony.Parsing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            number.Options = NumberOptions.AllowSign;
            var string_literal = new StringLiteral("string", "'", StringOptions.AllowsDoubledQuote);
            string_literal.AddStartEnd("N'", "'", StringOptions.AllowsDoubledQuote | StringOptions.AllowsLineBreak);
            var Id_simple = TerminalFactory.CreateSqlExtIdentifier(this, "id_simple"); //covers normal identifiers (abc) and quoted id's ([abc d], "abc d")
            Id_simple.AddPrefix("@@", IdOptions.NameIncludesPrefix);
            Id_simple.AddPrefix("@", IdOptions.NameIncludesPrefix);

            var comma = ToTerm(",");
            var dot = ToTerm(".");
            var equals = ToTerm("=");
            var plus = ToTerm("+");
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
            var PROCEDURE = ToTerm("PROCEDURE");
            var DECLARE = ToTerm("DECLARE");
            var TRY = ToTerm("TRY");
            var CATCH = ToTerm("CATCH");
            var CAST = ToTerm("CAST");
            var AND = ToTerm("AND");
            var OR = ToTerm("OR");
            var GRANT = ToTerm("GRANT");
            var UNION = ToTerm("UNION");
            var ALL = ToTerm("ALL");
            var CASE = ToTerm("CASE");
            var WHEN = ToTerm("WHEN");
            var THEN = ToTerm("THEN");
            var RETURN = ToTerm("RETURN");
            var COMMIT = ToTerm("COMMIT");
            var TRAN = ToTerm("TRAN");
            var TRANSACTION = ToTerm("TRANSACTION");
            var TOP = ToTerm("TOP");
            var MERGE = ToTerm("MERGE");
            var USING = ToTerm("USING");
            var MATCHED = ToTerm("MATCHED");
            var TARGET = ToTerm("TARGET");
            var TRUNCATE = ToTerm("TRUNCATE");
            var ROLLBACK = ToTerm("ROLLBACK");
            var STATISTICS = ToTerm("STATISTICS");
            var ROLE = ToTerm("ROLE");
            var WHILE = ToTerm("WHILE");
            var BREAK = ToTerm("BREAK");
            var REBUILD = ToTerm("REBUILD");
            var CHECKPOINT = ToTerm("CHECKPOINT");
            var HASH = ToTerm("HASH");

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
            var constraintListOpt = new NonTerminal("constraintListOpt");
            var constraintDef = new NonTerminal("constraintDef");
            var indexContraintDef = new NonTerminal("indexContraintDef");
            var constraintTypeOpt = new NonTerminal("constraintTypeOptOpt");
            var defaultValueOpt = new NonTerminal("defaultValueOpt");
            var idlist = new NonTerminal("idlist");
            var idOrExpression = new NonTerminal("idOrExpression");
            var idOrExpressionList = new NonTerminal("idOrExpressionList");
            var idlistForSelect = new NonTerminal("idlistForSelect");
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
            var betweenClauseOpt = new NonTerminal("betweenClauseOpt");
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
            var beginTryCatchStmt = new NonTerminal("beginTryCatchStmt");
            var beginEndStmtList = new NonTerminal("beginEndStmtList");
            var isNullOpt = new NonTerminal("isNullOpt");
            var setStmtOpt = new NonTerminal("setStmtOpt");
            var useStmt = new NonTerminal("useStmt");
            var forOpt = new NonTerminal("forOpt");
            var execStmt = new NonTerminal("execStmt");
            var cascadeOpt = new NonTerminal("cascadeOpt");
            var cascadeListOpt = new NonTerminal("cascadeListOpt");
            var alterProcedureStmt = new NonTerminal("alterProcedureStmt");
            var declareStmt = new NonTerminal("declareStmt");
            var concatStringItem = new NonTerminal("concatStringItem");
            var concatStringList = new NonTerminal("concatStringList");
            var castFunCall = new NonTerminal("castFunCall");
            var funCallList = new NonTerminal("funCallList");
            var funcallDelim = new NonTerminal("funcallDelim");
            var declareList = new NonTerminal("declareList");
            var declareListItem = new NonTerminal("declareListItem");
            var grantStmt = new NonTerminal("grantStmt");
            var joinChainOptList = new NonTerminal("joinChainOptList");
            var leftParenOpt = new NonTerminal("leftParenOpt");
            var rightParenOpt = new NonTerminal("rightParenOpt");
            var unionAllOpt = new NonTerminal("unionAllOpt");
            var selectCaseStmt = new NonTerminal("selectCaseStmt");
            var unionAllListOpt = new NonTerminal("unionAllListOpt");
            var returnStmt = new NonTerminal("returnStmt");
            var beginTransStmt = new NonTerminal("beginTransStmt");
            var rollbackTransStmt = new NonTerminal("rollbackTransStmt");
            var topOpt = new NonTerminal("topOpt");
            var mergeStmt = new NonTerminal("mergeStmt");
            var mergeWhenMatched = new NonTerminal("mergeWhenMatched");
            var mergeWhenNotMatched = new NonTerminal("mergeWhenNotMatched");
            var truncateStmt = new NonTerminal("truncateStmt");
            var commitTransStmt = new NonTerminal("commitTransStmt");
            var noLockOpt = new NonTerminal("noLockOpt");
            var declareTableStmt = new NonTerminal("declareTableStmt");
            var joinStmtOpt = new NonTerminal("joinStmtOpt");
            var forXmlStmtOpt = new NonTerminal("forXmlStmtOpt");
            var forXmlFunCallList = new NonTerminal("forXmlFunCallList");
            var funArgsList = new NonTerminal("funArgsList");
            var updateStatisticsStmt = new NonTerminal("updateStatisticsStmt");
            var createRoleStmt = new NonTerminal("createRoleStmt");
            var whileStmt = new NonTerminal("whileStmt");
            var alterIndexStmt = new NonTerminal("alterIndexStmt");
            var ifCondition = new NonTerminal("ifCondition");
            var ifConditionChain = new NonTerminal("ifConditionChain");
            var hashOpt = new NonTerminal("hashOpt");
            var IdAsType = new NonTerminal("IdAsType");
            var selectWithUnion = new NonTerminal("selectWithUnion");
            var withStmt = new NonTerminal("withStmt");

            //BNF Rules
            this.Root = stmtList;
            stmtLine.Rule = stmt + semiOpt;
            semiOpt.Rule = Empty | ";";
            stmtList.Rule = MakePlusRule(stmtList, stmtLine);
            setStmtOpt.Rule = Empty | SET + Id + Id | SET + Id + equals + (leftParenOpt + selectStmt + rightParenOpt | Id | funCall | concatStringList | expression);
            useStmt.Rule = USE + Id;
            execStmt.Rule = EXEC + (Empty | Id | Id + ".." + Id) + (leftParenOpt + funArgsList + rightParenOpt);
            declareStmt.Rule = DECLARE + declareList;
            declareTableStmt.Rule = DECLARE + Id + TABLE + "(" + fieldDefList + ")";
            declareListItem.Rule = Id + typeName
                                 | Id + typeName + equals + term;
            declareList.Rule = MakePlusRule(declareList, comma, declareListItem);
            castFunCall.Rule = CAST + "(" + funCall + asOpt + (Empty | typeName) + ")" + asOpt + (Empty | typeName);
            grantStmt.Rule = GRANT + term + ON + TYPE + "::" + Id + "TO" + Id;
            returnStmt.Rule = RETURN + term;
            leftParenOpt.Rule = Empty | "(";
            rightParenOpt.Rule = Empty | ")";
            unionAllOpt.Rule = Empty | UNION + ALL + leftParenOpt + selectStmt + rightParenOpt;
            unionAllListOpt.Rule = MakeStarRule(unionAllListOpt, unionAllOpt);
            idOrExpression.Rule = Id | expression;
            idOrExpressionList.Rule = MakeStarRule(idOrExpressionList, comma, idOrExpression);
            whileStmt.Rule = WHILE + expression + beginEndStmt;

            //ID
            Id.Rule = MakePlusRule(Id, dot, Id_simple);
            IdWithAliasOpt.Rule = Id | Id + Id | Id + AS + Id;
            IdAsType.Rule = Id + AS + typeName;
            concatStringItem.Rule = leftParenOpt + term + rightParenOpt;
            concatStringList.Rule = MakePlusRule(concatStringList, plus, concatStringItem);

            stmt.Rule = createTableStmt | createIndexStmt | createViewStmt | createTypeStmt | createRoleStmt
                      | declareTableStmt | alterStmt | dropTableStmt | dropIndexStmt | dropViewStmt
                      | selectWithUnion | insertStmt | updateStmt | deleteStmt | whileStmt
                      | GO | ifStmt | elseStmt | beginEndStmt | printStmt | withStmt
                      | execStmt | setStmtOpt | useStmt | funCall | declareStmt | returnStmt
                      | grantStmt | mergeStmt | truncateStmt | updateStatisticsStmt
                      | beginTransStmt | commitTransStmt | rollbackTransStmt
                      | BREAK | CHECKPOINT
                      | ";";

            onOpt.Rule = Empty | ON + Id;
            textImageOnOpt.Rule = Empty | TEXTIMAGE_ON + Id;
            forOpt.Rule = Empty | FOR + Id;
            onListOpt.Rule = MakePlusRule(onListOpt, onOpt);
            withStmt.Rule = WITH + Id + AS + leftParenOpt + selectStmt + rightParenOpt;
            fieldDefLists.Rule = MakePlusRule(fieldDefLists, fieldDefList);

            printStmt.Rule = PRINT + concatStringList;

            beginEndStmtList.Rule = MakePlusRule(beginEndStmtList, stmt);
            beginEndStmt.Rule = beginTryCatchStmt | BEGIN + beginEndStmtList + END;
            beginTryCatchStmt.Rule = BEGIN + TRY + beginEndStmtList + END + TRY + BEGIN + CATCH + beginEndStmtList + END + CATCH;
            beginTransStmt.Rule = BEGIN + (TRAN | TRANSACTION) + (Empty | Id);
            commitTransStmt.Rule = COMMIT + (TRAN | TRANSACTION) + (Empty | Id);
            rollbackTransStmt.Rule = ROLLBACK + (TRAN | TRANSACTION) + (Empty | Id);
            truncateStmt.Rule = TRUNCATE + TABLE + Id;
            isNullOpt.Rule = Empty | IS + NULL;

            funcallDelim.Rule = AND | OR;
            funCallList.Rule = MakePlusRule(funCallList, funcallDelim, funCall);

            // If
            ifStmt.Rule = IF + leftParenOpt + ifConditionChain + rightParenOpt;
            ifCondition.Rule = (notOpt + funCall + isNullOpt | NOT + leftParenOpt + expression + rightParenOpt)
                | (Empty | "EXISTS") + "(" + (Id_simple + IS + NULL | settingListItem | selectWithUnion) + ")"
                | "EXISTS" + "(" + selectWithUnion + ")"
                | (Id_simple + IS + (Empty | NOT) + NULL | settingListItem)
                | expression;
            ifConditionChain.Rule = MakePlusRule(ifConditionChain, AND | OR, ifCondition);
            elseStmt.Rule = ELSE;

            createRoleStmt.Rule = CREATE + ROLE + Id;
            createViewStmt.Rule = CREATE + VIEW + Id + AS + leftParenOpt + selectWithUnion + rightParenOpt + unionAllListOpt;
            createTypeStmt.Rule = CREATE + TYPE + Id + FROM + Id
                                | CREATE + TYPE + Id + AS + TABLE + "(" + fieldDefLists + ")";

            //Create table
            createTableStmt.Rule = CREATE + TABLE + Id + "(" + fieldDefLists + ")" + (onOpt | withClauseOpt) + textImageOnOpt;
            fieldDefList.Rule = MakeListRule(fieldDefList, comma, fieldDef, TermListOptions.AllowTrailingDelimiter | TermListOptions.PlusList);
            fieldDef.Rule = columnDef | constraintListOpt;
            columnDef.Rule = Id + typeName + collateOpt + primaryKeyOpt + nullSpecOpt + referencesOpt + defaultValueOpt + withClauseOpt
                | Id + typeName + collateOpt + primaryKeyOpt + nullSpecOpt + constraintListOpt + withClauseOpt
                | Id + typeName + collateOpt + primaryKeyOpt + notForReplOpt + nullSpecOpt + defaultValueOpt + withClauseOpt
                | Id + typeName + collateOpt + primaryKeyOpt + notForReplOpt + nullSpecOpt + constraintListOpt + withClauseOpt
                | Id + typeName + equals + term
                | primaryKeyOpt + indexTypeOpt + idlistParOpt + withClauseOpt
                | term;
            referencesOpt.Rule = Empty | "References" + Id + idlistParOpt;
            notForReplOpt.Rule = Empty | (NOT + FOR + REPLICATION);
            nullSpecOpt.Rule = Empty | (NOT + FOR + REPLICATION) | NULL | NOT + NULL | NOT + NULL + typeName | NULL + typeName;
            collateOpt.Rule = Empty | COLLATE + Id_simple;
            identityOpt.Rule = Empty | IDENTITY;
            typeNameParamsList.Rule = MakePlusRule(typeNameParamsList, comma, term);
            typeName.Rule = Id_simple | Id_simple + "(" + typeNameParamsList + ")" | Id_simple + "(max)";
            constraintDef.Rule = CONSTRAINT + Id + constraintTypeOpt + onListOpt;
            indexContraintDef.Rule = constraintTypeOpt + onListOpt;
            constraintListOpt.Rule = MakeStarRule(constraintListOpt, constraintDef);
            constraintTypeOpt.Rule = Empty | defaultValueOpt + withClauseOpt
                | primaryKeyOpt + indexTypeList + idlistParOpt + withClauseOpt
                | CHECK + "(" + expression + ")" + withClauseOpt
                | NOT + NULL + idlistParOpt + withClauseOpt
                | "Foreign" + KEY + idlistParOpt + referencesOpt + notForReplOpt + withClauseOpt
                | "INCLUDE" + idlistParOpt + withClauseOpt + onOpt;
            idlistParOpt.Rule = Empty | "(" + orderList + ")";
            idlist.Rule = MakePlusRule(idlist, comma, Id);
            idlistForSelect.Rule = MakePlusRule(idlist, comma, IdWithAliasOpt);
            defaultValueParamsList.Rule = MakePlusRule(defaultValueParamsList, comma, term);
            defaultValueOpt.Rule = Empty | (DEFAULT + defaultValueParams);
            defaultValueParams.Rule = term | "(" + term + ")";

            //Create Index
            primaryKeyOpt.Rule = Empty | PRIMARY + KEY | typeName;
            createIndexStmt.Rule = CREATE + indexTypeList + INDEX + Id + onOpt + "(" + orderList + ")" + constraintTypeOpt + whereClauseOpt + withClauseOpt + onOpt;
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
            noLockOpt.Rule = (Empty | WITH + leftParenOpt + "NOLOCK" + rightParenOpt);

            //Alter 
            alterStmt.Rule = ALTER + (TABLE | DATABASE) + Id + setStmtOpt + alterCmdOpt
                             | alterProcedureStmt
                             | alterIndexStmt;
            alterCmdOpt.Rule = Empty | ADD + COLUMN + fieldDefList + constraintDef
                          | CHECK + CONSTRAINT + Id
                          | WITH + (CHECK | NOCHECK) + ADD + CONSTRAINT + Id + constraintTypeOpt + cascadeListOpt
                          | ADD + constraintDef + forOpt
                          | DROP + COLUMN + Id
                          | DROP + CONSTRAINT + Id;
            alterProcedureStmt.Rule = ALTER + PROCEDURE + Id + leftParenOpt + fieldDefLists + rightParenOpt + asOpt + beginEndStmt;
            alterIndexStmt.Rule = ALTER + INDEX + Id + ON + Id + REBUILD + WITH + "(" + settingList + ")";

            //Drop stmts
            dropTableStmt.Rule = DROP + TABLE + Id;
            dropIndexStmt.Rule = DROP + INDEX + Id + ON + Id;
            dropViewStmt.Rule = DROP + VIEW + Id;

            //Insert stmt
            insertStmt.Rule = INSERT + (Empty | intoOpt + Id) + (idlistParOpt + insertData | execStmt);
            insertData.Rule = leftParenOpt + selectWithUnion + rightParenOpt | VALUES + "(" + exprList + ")";
            intoOpt.Rule = Empty | INTO; //Into is optional in MSSQL

            //Update stmt
            updateStmt.Rule = UPDATE + topOpt + (Empty | Id) + SET + assignList + fromClauseOpt + joinStmtOpt + whereClauseOpt + andClauseOpt;
            assignList.Rule = MakePlusRule(assignList, comma, assignment);
            assignment.Rule = Id + "=" + expression;
            updateStatisticsStmt.Rule = UPDATE + STATISTICS + Id;

            //Delete stmt
            deleteStmt.Rule = DELETE + (Empty | FROM) + Id + whereClauseOpt + andClauseOpt;

            //Select stmt
            selectCaseStmt.Rule = CASE + WHEN + leftParenOpt + expression + rightParenOpt + THEN + term + ELSE + Id + END + (Empty | asOpt + Id);
            selectStmt.Rule =  SELECT + topOpt + selRestrOpt + selList + intoClauseOpt + fromClauseOpt + forXmlStmtOpt + joinChainOptList + whereClauseOpt + andClauseOpt +
                              betweenClauseOpt + groupClauseOpt + havingClauseOpt + orderClauseOpt;
            selectWithUnion.Rule = MakePlusRule(selectWithUnion, UNION, selectStmt);
            mergeStmt.Rule = MERGE + Id + AS + Id + USING + (Empty | Id) + leftParenOpt + (Empty | selectWithUnion) + rightParenOpt + AS + Id + ON + expression + mergeWhenMatched +
                                mergeWhenNotMatched + mergeWhenNotMatched;
            mergeWhenMatched.Rule = WHEN + MATCHED + andClauseOpt + THEN + stmt;
            mergeWhenNotMatched.Rule = Empty | WHEN + NOT + MATCHED + BY + Id + THEN + stmt;
            forXmlStmtOpt.Rule = Empty | FOR + "XML" + forXmlFunCallList;
            forXmlFunCallList.Rule = MakePlusRule(forXmlFunCallList, comma, funCall);

            topOpt.Rule = Empty | TOP + leftParenOpt + (number | Id) + rightParenOpt;
            selRestrOpt.Rule = Empty | "ALL" | "DISTINCT";
            selList.Rule = columnItemList + semiOpt | "*";
            columnItemList.Rule = MakePlusRule(columnItemList, comma, columnItem);
            columnItem.Rule = columnSource;
            aliasOpt.Rule = Empty | asOpt + Id;
            asOpt.Rule = Empty | AS;
            columnSource.Rule = Id + aliasOpt | Id + "=" + (selectCaseStmt | concatStringList | expression) | expression | expression + asOpt + (Empty | Id) | selectCaseStmt;
            aggregate.Rule = aggregateName + "(" + aggregateArg + ")";
            aggregateArg.Rule = expression | "*";
            aggregateName.Rule = COUNT | "Avg" | "Min" | "Max" | "StDev" | "StDevP" | "Sum" | "Var" | "VarP";
            intoClauseOpt.Rule = Empty | INTO + Id;
            fromClauseOpt.Rule = Empty | FROM + leftParenOpt + (selectStmt | funCall | idlistForSelect) + rightParenOpt + (Empty | AS + Id) + noLockOpt;
            joinStmtOpt.Rule = Empty | JOIN + Id + asOpt + Id + noLockOpt + ON + expression;
            joinChainOpt.Rule = Empty | joinKindOpt + hashOpt + joinStmtOpt;
            joinChainOptList.Rule = MakeStarRule(joinChainOptList, joinChainOpt);
            joinKindOpt.Rule = Empty | "INNER" | "OUTER" | "LEFT" | "RIGHT";
            hashOpt.Rule = Empty | HASH;
            whereClauseOpt.Rule = Empty | "WHERE" + expression;
            andClauseOpt.Rule = Empty | "AND" + expression;
            betweenClauseOpt.Rule = Empty | "BETWEEN" + expression;
            groupClauseOpt.Rule = Empty | "GROUP" + BY + idOrExpressionList;
            havingClauseOpt.Rule = Empty | "HAVING" + expression;
            orderClauseOpt.Rule = Empty | "ORDER" + BY + orderList;

            //Expression
            exprList.Rule = MakePlusRule(exprList, comma, expression);
            expression.Rule = term | unExpr | binExpr | betweenExpr; //-- BETWEEN doesn't work - yet; brings a few parsing conflicts 
            term.Rule = Id | IdAsType | string_literal | number | funCall | tuple | aggregate;// | inStmt;
            tuple.Rule = "(" + exprList + ")";
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
            funCall.Rule = Id + "(" + funArgsList + ")" + (Empty | AS + typeName);
            funArgsOpt.Rule = Empty | selectStmt | expression | string_literal | Id + "=" + term;
            funArgsList.Rule = MakePlusRule(funArgsList, comma, funArgsOpt);
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
}
