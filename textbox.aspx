<%@ Page Language="C#" AutoEventWireup="true" CodeFile="textbox.aspx.cs" Inherits="textbox" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<meta http-equiv="Content-Type" content="text/html; charset=utf-8"/>
    <script src="Scripts/js/tinymce.min.js"></script>
     <script type="text/javascript">
      tinymce.init({
        selector: '#mytextarea'
      });
  </script>
    <title></title>
</head>
<body>
    <form method="post">
        <textarea id="mytextarea">Hello, World!</textarea>
    </form>
</body>
</html>
