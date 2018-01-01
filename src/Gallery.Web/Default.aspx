<%@ Page Language="C#" AutoEventWireup="true" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
	<title>MobileMe Gallery</title>
	<style type="text/css">
		html, body
		{
			height: 100%;
			overflow: auto;
		}
		body
		{
			padding: 0;
			margin: 0;
		}
		#silverlightControlHost
		{
			height: 100%;
			text-align:center;
		}
	</style>
	<script type="text/C#" runat="server">
		void Page_Load()
		{
			if (!HttpContext.Current.Items.Contains("username"))
			{
				Response.Redirect(string.Format("/{0}", ConfigurationManager.AppSettings["DefaultUsername"]));
			}
		}
	</script>
	<script type="text/javascript" src="Silverlight.js"></script>
	<script type="text/javascript">
		function onSilverlightError(sender, args) {
			var appSource = "";
			if (sender != null && sender != 0) {
				appSource = sender.getHost().Source;
			}

			var errorType = args.ErrorType;
			var iErrorCode = args.ErrorCode;

			if (errorType == "ImageError" || errorType == "MediaError") {
				return;
			}

			var errMsg = "Unhandled Error in Silverlight Application " + appSource + "\n";

			errMsg += "Code: " + iErrorCode + "    \n";
			errMsg += "Category: " + errorType + "       \n";
			errMsg += "Message: " + args.ErrorMessage + "     \n";

			if (errorType == "ParserError") {
				errMsg += "File: " + args.xamlFile + "     \n";
				errMsg += "Line: " + args.lineNumber + "     \n";
				errMsg += "Position: " + args.charPosition + "     \n";
			}
			else if (errorType == "RuntimeError") {
				if (args.lineNumber != 0) {
					errMsg += "Line: " + args.lineNumber + "     \n";
					errMsg += "Position: " + args.charPosition + "     \n";
				}
				errMsg += "MethodName: " + args.methodName + "     \n";
			}

			throw new Error(errMsg);
		}
	</script>
</head>
<body>
	<form id="form1" runat="server" style="height:100%">
		<div id="silverlightControlHost">
			<object data="data:application/x-silverlight-2," type="application/x-silverlight-2" width="100%" height="100%">
				<param name="source" value="ClientBin/Gallery.xap"/>
				<param name="onError" value="onSilverlightError" />
				<param name="background" value="white" />
				<param name="minRuntimeVersion" value="4.0.50524.0" />
				<param name="autoUpgrade" value="true" />
				<param name="initParams" value="CurrentUsername=<%Response.Write(HttpContext.Current.Items["username"].ToString());%>,ConfigServiceUri=<%Response.Write(ConfigurationManager.AppSettings["ConfigServiceUri"]);%>" />
					<table style="height:100%;width:100%;background-color:#000;color:#fff;">
						<tbody>
							<tr>
								<td valign="middle" align="center">
									<table style="background:url('empty_gallery_bg.png');height:279px;width:328px;">
										<tbody>
											<tr>
												<td align="center" valign="bottom">
													<table style="background-image:url('empty_gallery_graphic.png');height:236px;width:219px;margin:10px;">
														<tbody>
															<tr>
																<td align="center" valign="bottom">
																	<span style="color:#fff;font-weight:bold;font-family:Verdana;font-size:10pt;">Silverlight is required. <br /><a href="http://go.microsoft.com/fwlink/?LinkID=149156&v=4.0.50524.0" style="color:#33B6FF">Click Here to Install Silverlight</a></span><br /><br />
																</td>
															</tr>
														</tbody>
													</table>
												</td>
											</tr>
										</tbody>
									</table>
								</td>
							</tr>
						</tbody>
					</table>
			</object>
			<iframe id="_sl_historyFrame" style="visibility:hidden;height:0px;width:0px;border:0px"></iframe>
		</div>
	</form>
</body>
</html>
