
$(function () {
    $.initCommonLib();
    $.loadNewNotifyList();
    setInterval(function () { $.loadNewNotifyList(); }, 3000);
});