CxList methods = Find_Methods();

// org.springframework.web.util.HtmlUtils
result.Add(methods.FindByMemberAccess("HtmlUtils.htmlEscape*"));
// org.apache.commons.lang(3).StringEscapeUtil
result.Add(methods.FindByMemberAccess("StringEscapeUtils.escapeHtml*"));
result.Add(methods.FindByMemberAccess("StringEscapeUtils.escapeXml*"));
// org.apache.struts.util.ResponseUtils
result.Add(methods.FindByMemberAccess("ResponseUtils.filter"));