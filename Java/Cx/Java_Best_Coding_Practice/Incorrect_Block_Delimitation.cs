CxList iterationStmt = Find_IterationStmt();
string regexPrefix = @"[\s\n};]?";
string balancedParentheses = @"([^()]|(?<open>\()|(?<-open>\)))+";
string testIfBalanced = @"(?(open)(?!))";
CxList.CxRegexOptions regexOptions = CxList.CxRegexOptions.DoNotSearchInStringLiterals | CxList.CxRegexOptions.AllowOverlaps;

CxList ifStmt = Find_Ifs().FindByRegex(regexPrefix + @"if(\s)*\(" + balancedParentheses + @"\)[\s\n]*[^{\s\n]" + testIfBalanced, regexOptions);
CxList elseStmt = All.FindByRegex(regexPrefix + @"else(\s)*[^{]*;", regexOptions);
CxList elseStmtMulti = All.FindByRegex(regexPrefix + @"(?<=else(\s)*\n)[^{;]+[;)]", regexOptions);
CxList whileStmt = iterationStmt.FindByRegex(regexPrefix + @"while(\s)*\(" + balancedParentheses + @"\)[\s\n]*[^{\s\n;]" + testIfBalanced, regexOptions);
CxList forStmt = iterationStmt.FindByRegex(regexPrefix + @"for(\s)*\(" + balancedParentheses + @"\)[\s\n]*[^{\s\n;]" + testIfBalanced, regexOptions);
CxList doWhileStmt = iterationStmt.FindByRegex(regexPrefix + @"do[\s\n]+[^{\s\n]", regexOptions);

result.Add(ifStmt);
result.Add(elseStmt);
result.Add(elseStmtMulti);
result.Add(whileStmt);
result.Add(forStmt);
result.Add(doWhileStmt);
result -= Find_Properties_Files();