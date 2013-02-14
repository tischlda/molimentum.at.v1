<%@ Page Language="C#" Inherits="System.Web.UI.Page" ContentType="application/javascript" %>
$(function() {
    $('a[rel*=lightbox]').lightbox({
        show_helper_text: false,
        show_info: true,
        show_extended_info: true,
        download_link: false,
        ie6_upgrade: true,
        speed: 10
    });
});