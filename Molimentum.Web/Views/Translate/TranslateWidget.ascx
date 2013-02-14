<%@ Control Language="C#" Inherits="System.Web.Mvc.ViewUserControl" %>
<div class="widget">
    <span id="google_translate_element" class="widget" style="padding-left: -20px"></span>
    <script>
        function googleTranslateElementInit() {
            new google.translate.TranslateElement({ pageLanguage: "de" }, "google_translate_element");
            $("#widgets").css("right", "0");
        };
    </script>
    <script src="http://translate.google.com/translate_a/element.js?cb=googleTranslateElementInit"></script>
</div>