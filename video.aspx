<%@ Page Language="C#" AutoEventWireup="true" CodeFile="video.aspx.cs" Inherits="video" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
      <object classid="clsid:D27CDB6E-AE6D-11cf-96B8-444553540000" codebase="http://download.macromedia.com/pub/shockwave/cabs/flash/swflash.cab#version=8,0,0,0" width="200" height="200" id="FLVPlayer">
        <param name="movie" value="FLVPlayer_Progressive.swf" />
        <param name="quality" value="high" />
        <param name="wmode" value="opaque" />
        <param name="scale" value="noscale" />
        <param name="salign" value="lt" />
        <param name="FlashVars" value="&amp;MM_ComponentVersion=1&amp;skinName=Clear_Skin_1&amp;streamName=video/Italy&amp;autoPlay=true&amp;autoRewind=true" />
        <embed src="FLVPlayer_Progressive.swf" flashvars="&amp;MM_ComponentVersion=1&amp;skinName=Clear_Skin_1&amp;streamName=video/Italy&amp;autoPlay=true&amp;autoRewind=true" quality="high" wmode="opaque" scale="noscale" width="200" height="200" name="FLVPlayer" salign="lt" type="application/x-shockwave-flash" pluginspage="http://www.adobe.com/shockwave/download/download.cgi?P1_Prod_Version=ShockwaveFlash"></embed>
      </object>
    </div>
    </form>
</body>
</html>
