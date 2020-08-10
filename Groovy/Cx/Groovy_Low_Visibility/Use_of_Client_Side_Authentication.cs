CxList htmlOutput = Find_Web_Outputs() + Find_Html_Outputs();

CxList strings = Find_Strings();
CxList htmlRemarks = strings.FindByName("*<script*") + strings.FindByName("*</script>*");

htmlOutput = htmlOutput.DataInfluencedBy(htmlRemarks);

CxList pass = Find_Password_Strings();

result = htmlOutput.DataInfluencedBy(pass);