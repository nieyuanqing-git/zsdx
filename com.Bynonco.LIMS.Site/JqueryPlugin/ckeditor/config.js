/*
Copyright (c) 2003-2010, CKSource - Frederico Knabben. All rights reserved.
For licensing, see LICENSE.html or http://ckeditor.com/license
*/

CKEDITOR.editorConfig = function (config) {
    // Define changes to default configuration here. For example:
    // config.language = 'fr';
    // config.uiColor = '#AADC6E';

    //    config.toolbar_Full.pop(['lineheight']);

    config.filebrowserBrowseUrl = '/JqueryPlugin/Ckfinder/ckfinder.html';
    config.filebrowserImageBrowseUrl = '/JqueryPlugin/Ckfinder/ckfinder.html?Type=Images';
    config.filebrowserFlashBrowseUrl = '/JqueryPlugin/Ckfinder/ckfinder.html?Type=Flash';
    config.filebrowserUploadUrl = '/JqueryPlugin/Ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Files';
    config.filebrowserImageUploadUrl = '/JqueryPlugin/Ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Images';
    config.filebrowserFlashUploadUrl = '/JqueryPlugin/Ckfinder/core/connector/aspx/connector.aspx?command=QuickUpload&type=Flash';
    //加中文字体:
    config.font_names = "宋体/宋体;黑体/黑体;仿宋/仿宋_GB2312;楷体/楷体_GB2312;隶书/隶书;幼圆/幼圆;微软雅黑/微软雅黑;" + config.font_names;

    //MS word:
    config.pasteFromWordRemoveFontStyles = false;
    config.pasteFromWordRemoveStyles = false;


    //===========================================
    //    config.width = "960px";
    config.resize_enabled = false;
    config.height = "250px";
    config.width = "600px";
    //===========================================



    //新增行距
    //    config.extraPlugins = 'lineheight';
    //    //    config.toolbar_Full+=(config.extraPlugins ? ',lineheight' : 'lineheight');
    //    config.toolbar_Full.push(['lineheight']);

    config.toolbar = 'MyToolbar';

    config.toolbar_MyToolbar =
    [
        ['Source', '-', 'Save', 'NewPage', 'Preview', '-', 'Templates'],
        ['Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord'],
        ['Undo', 'Redo', '-', 'Find', 'Replace', '-', 'SelectAll', 'RemoveFormat', '-', 'Outdent', 'Indent', 'Blockquote'],
        '/',
        ['Bold', 'Italic', 'Underline', 'Strike', '-', 'Subscript', 'Superscript'],
        ['NumberedList', 'BulletedList'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock'],
        ['Link', 'Unlink', 'Anchor'],
        ['Image', 'Flash', 'flvPlayer', 'Table', 'HorizontalRule', 'Smiley', 'SpecialChar'],
        '/',
        ['Styles', 'Format', 'Font', 'FontSize', 'lineheight'],
        ['TextColor', 'BGColor'],
        ['Maximize', 'ShowBlocks']

    ];
    config.extraPlugins = 'flvPlayer,lineheight';
};
