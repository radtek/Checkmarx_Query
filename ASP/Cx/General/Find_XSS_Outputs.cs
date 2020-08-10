CxList outputs =	All.FindByMemberAccess("label.text") + 
				All.FindByMemberAccess("body.tagname") + 
				All.FindByMemberAccess("body.id") + 
				All.FindByMemberAccess("body.innerhtml") + 
				All.FindByMemberAccess("applet.id") + 
				All.FindByMemberAccess("applet.innerhtml") + 
				All.FindByMemberAccess("applet.tagname") + 
				All.FindByMemberAccess("embed.id") + 
				All.FindByMemberAccess("embed.innerhtml") + 
				All.FindByMemberAccess("embed.tagname") + 
				All.FindByMemberAccess("html.id") + 
				All.FindByMemberAccess("html.innerhtml") + 
				All.FindByMemberAccess("html.tagname") + 
				All.FindByMemberAccess("iframe.id") + 
				All.FindByMemberAccess("iframe.innerhtml") + 
				All.FindByMemberAccess("iframe.tagname") + 
				All.FindByMemberAccess("meta.id") +
				All.FindByMemberAccess("label.id") + 
				All.FindByMemberAccess("textbox.id") + 
				All.FindByMemberAccess("button.id") + 
				All.FindByMemberAccess("linkbutton.text") + 
				All.FindByMemberAccess("linkbutton.id") + 
				All.FindByMemberAccess("imagebutton.id") + 
				All.FindByMemberAccess("hyperlink.id") + 
				All.FindByMemberAccess("hyperlink.text") + 
				All.FindByMemberAccess("hyperlink.target") + 
				All.FindByMemberAccess("dropdownlist.id") + 
				All.FindByMemberAccess("listbox.id") + 
				All.FindByMemberAccess("checkbox.text") + 
				All.FindByMemberAccess("checkbox.id") + 
				All.FindByMemberAccess("checkboxlist.id") + 
				All.FindByMemberAccess("checkboxlist1.items") + 
				All.FindByMemberAccess("radiobutton.text") + 
				All.FindByMemberAccess("radiobutton.id") + 
				All.FindByMemberAccess("radiobutton.groupname") + 
				All.FindByMemberAccess("radiobuttonlist.id") + 
				All.FindByMemberAccess("radiobuttonlist.items") + 
				All.FindByMemberAccess("image.id") + 
				All.FindByMemberAccess("imagemap.id") + 
				All.FindByMemberAccess("table.caption") + 
				All.FindByMemberAccess("table.id") + 
				All.FindByMemberAccess("bulletedlist.id") + 
				All.FindByMemberAccess("hiddenfield.id") + 
				All.FindByMemberAccess("literal.id") + 
				All.FindByMemberAccess("literal.text") + 
				All.FindByMemberAccess("calendar.id") + 
				All.FindByMemberAccess("calendar.caption") + 
				All.FindByMemberAccess("calendar.nextmonthtext") + 
				All.FindByMemberAccess("calendar.prevmonthtext") + 
				All.FindByMemberAccess("adrotator.id") + 
				All.FindByMemberAccess("adrotator.target") + 
				All.FindByMemberAccess("fileupload.id") + 
				All.FindByMemberAccess("wizard.id") + 
				All.FindByMemberAccess("wizard.headertext") + 
				All.FindByMemberAccess("panel.id") + 
				All.FindByMemberAccess("localize.text") + 
				All.FindByMemberAccess("input.id") + 
				All.FindByMemberAccess("input.id") + 
				All.FindByMemberAccess("textarea.id") + 
				All.FindByMemberAccess("table.id") + 
				All.FindByMemberAccess("img.id") + 
				All.FindByMemberAccess("img.src") + 
				All.FindByMemberAccess("select.id") + 
				All.FindByMemberAccess("hr.id") + 
				All.FindByMemberAccess("div.id") + 
				All.FindByMemberAccess("text.id") +
				All.FindByMemberAccess("password.id") +
				All.FindByMemberAccess("hidden.id") + 
				All.FindByMemberAccess("htmlinputpassword.id") + 
				All.FindByMemberAccess("htmlinputtext.id") +
				All.FindByMemberAccess("htmlinputfile.id") + 
				All.FindByMemberAccess("htmlselect.id") + 
				All.FindByMemberAccess("htmltextarea.id") + 
				All.FindByMemberAccess("httpresponse.*") + 
	
				All.FindByMemberAccess("hr.innerhtml") + 
				All.FindByMemberAccess("div.innerhtml") + 
			
				All.FindByMemberAccess("htmltextwriter.write") + 
				All.FindByMemberAccess("label.text") + 
				All.FindByMemberAccess("hyperlink.text") + 
				All.FindByMemberAccess("attributecollection.add") + 
				All.FindByMemberAccess("attributecollection.item") + 
				All.FindByMemberAccess("image.imageurl") + 
				All.FindByMemberAccess("hyperlink.navigateurl") + 
				All.FindByMemberAccess("htmlinputcontrol.value") + 
				All.FindByMemberAccess("htmlcontainercontrol.innerhtml") + 
				All.FindByMemberAccess("htmlcontainercontrol.innertext") + 
				All.FindByMemberAccess("clientscriptmanager.registerstartupscript") + 
				All.FindByMemberAccess("page.registerstartupscript") + 
				All.FindByMemberAccess("page.registerclientscriptblock") + 
				All.FindByMemberAccess("tablecell.text") + 
				All.FindByMemberAccess("linkbutton.text") + 
				All.FindByMemberAccess("literal.text") + 
				All.FindByMemberAccess("checkbox.text") + 
				All.FindByMemberAccess("control.id") + 
				All.FindByMemberAccess("radiobutton.groupname") + 
				All.FindByMemberAccess("calendar.caption") + 
				All.FindByMemberAccess("table.caption") + 
				All.FindByMemberAccess("panel.groupingtext") + 
				All.FindByMemberAccess("response.write");
				

CxList fathersWithoutAssignment = outputs.GetFathers() - 
		outputs.GetFathers().FindByType(typeof(AssignExpr));

result = outputs.FindByAssignmentSide(CxList.AssignmentSide.Left) + 
	outputs.FindByFathers(fathersWithoutAssignment) + 
	All.FindByName("response") + All.FindByName("page.response") +
	All.FindByName("response.write") + All.FindByName("response.writefile") + 
	All.FindByName("page.response.write") + All.FindByName("page.response.writefile") +
	All.FindByName("response.binarywrite") + 
	All.FindByName("page.response.binarywrite");