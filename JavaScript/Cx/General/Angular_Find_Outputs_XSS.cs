if(cxScan.IsFrameworkActive("Angular")) {
	CxList methods = Find_Methods();

	result.Add(methods.FindByMemberAccess("Renderer2.appendChild"));
	result.Add(methods.FindByMemberAccess("Renderer2.insertBefore"));
	result.Add(methods.FindByMemberAccess("Renderer2.setAttribute"));
	result.Add(methods.FindByMemberAccess("Renderer2.removeAttribute"));
	result.Add(methods.FindByMemberAccess("Renderer2.addClass"));
	result.Add(methods.FindByMemberAccess("Renderer2.removeClass"));
	result.Add(methods.FindByMemberAccess("Renderer2.setStyle"));
	result.Add(methods.FindByMemberAccess("Renderer2.removeStyle"));
	result.Add(methods.FindByMemberAccess("Renderer2.setProperty"));
	result.Add(methods.FindByMemberAccess("Renderer2.setValue"));
	
	CxList viewOutputStmt = Find_ViewOutputStmt();
	result.Add(All.GetByAncs(viewOutputStmt));
}