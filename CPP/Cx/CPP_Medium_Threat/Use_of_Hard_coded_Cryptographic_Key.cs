CxList emptyString = Find_Empty_Strings();
CxList NULL = All.FindByName("null");
CxList keys = All.FindByShortName("*ENC*", false).FindByShortName("*KEY*", false);

CxList key_in_lSide = keys.FindByAssignmentSide(CxList.AssignmentSide.Left);
CxList strLiterals = Find_PrimitiveExpr() - emptyString - NULL;
CxList lit_in_rSide = strLiterals.FindByAssignmentSide(CxList.AssignmentSide.Right);

result = key_in_lSide.FindByFathers(key_in_lSide.GetFathers() * lit_in_rSide.GetFathers());
CxList key_in_Field = All.GetByAncs(Find_FieldDecls() + Find_ConstantDecl())*keys;
result.Add(key_in_Field.DataInfluencedBy(strLiterals));