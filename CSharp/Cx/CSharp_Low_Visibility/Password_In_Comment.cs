List <string> fileMaskList = new List<string>{"*.cs","*.cshtml"};

// Possible delimiters that can go after a password - we only want passwords in formets like password: 45iy345
string potentialProblem = @"\s+[""']\w";
string definiteProblem = @"\s*[:={[-]";
string notPasswords = @"\s*((-?)->)";

// All possible passwords
string fullRegex = @"(?<=\W)(?<=[^""'])(password|psw|pwd|authKey|cipher|adgangskode|benutzerkennwort|chiffre|clave|" +
	@"codewort|contrasena|contrasenya|geheimcode|geslo|heslo|jelszo|kennwort|losenord|losung|losungswort|" + 
	@"lozinka|modpas|motdepasse|parol|parola|parole|pasahitza|pasfhocal|passe|passord|passwort|pasvorto|" + 
	@"paswoord|salasana|schluessel|schluesselwort|senha|sifre|wachtwoord|wagwoord|watchword|zugangswort|" + 
	@"PAROLACHIAVE|PAROLA CHIAVE|PAROLECHIAVI|PAROLE CHIAVI|paroladordine|verschluesselt|sisma)";

fullRegex += "(?!" + notPasswords + ")(?=" + definiteProblem + "|" + potentialProblem + ")";

// Find passwords in comments
result = All.FindByRegexExt(fullRegex, fileMaskList, 
	true, CxList.CxRegexOptions.SearchOnlyInComments, RegexOptions.IgnoreCase);