// Find ouputs from Swing because they are safe from XSS 
CxList unkRefs = Find_UnknownReference();
CxList imports = Find_Import();

CxList swingImportsToIgnore = imports.FindByName("javax.swing*");

List < string > swingSafeList = new List<string>{"JButton", "JCheckBox", "JCheckBoxMenuItem", "JComboBox", 
		"JFormattedTextField", "JLabel", "JMenu", "JMenuItem", "JPasswordField",
		"JPopupMenu", "JRadioButton", "JRadioButtonMenuItem", "JTextArea" ,
		"JTextField", "JColorChooser","JList","ImageIcon","JScrollbar","JOptionPane",
		"JFileChooser","JProgressBar","JSlider","JSpinner"};

// Get all swing outputs from files with the "javax.swing" import
CxList swingSafeOutputs = unkRefs.FindByFiles(swingImportsToIgnore).FindByTypes(swingSafeList.ToArray());

result = swingSafeOutputs;
result.Add(swingSafeOutputs.GetMembersOfTarget());