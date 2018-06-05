function Init() {

    //search table
    $('.search-task').keyup(function () {
        var input, filter, table, tr, td, i;
        input = this;
        filter = input.value.toUpperCase();
        table = $('.task-table > table')[0];
        tr = table.getElementsByTagName("tr");
        for (i = 0; i < tr.length; i++) {
            td = tr[i].getElementsByTagName("td");
            for (j = 1; j < td.length; j++) {
                var data = td[j]
                if (data) {
                    if (data.innerText.toUpperCase().indexOf(filter) > -1) {
                        tr[i].style.display = "";
                        break;
                    } else {
                        tr[i].style.display = "none";
                    }
                }
            }
        }
    });

    //load phrase
    $('.project-list').change(function () {
        var id = $(this).val();
        $.ajax({
            url: "task/phrase-list.html",
            data: { projectId: id },
            success: function (response) {
                var phrasePanel = $(".phrase-list-panel");
                phrasePanel.html('');
                phrasePanel.html(response);
                var phrase = $(".phrase-list > li > a").first();
                var phraseId = phrase.attr('data-id');
                HandleLoadTask(id, phraseId);           
            },
        });
    });

    //load task
    $(".phrase-list > li > a").click(function (e) {
        e.preventDefault();
        var phraseid = $(this).attr('data-id');
        var projectId = $('.project-list').val();
        HandleLoadTask(projectId, phraseid);
    });

    function HandleLoadTask(projectId, phraseid) {
        $.ajax({
            url: "task/task-list.html",
            data: { projectId: projectId, phraseId: phraseid },
            success: function (response) {
                var taskListPanel = $(".task-list-panel");
                taskListPanel.html('');
                taskListPanel.html(response);            

                $('.task-table input[type="checkbox"]').iCheck({
                    checkboxClass: 'icheckbox_flat-green',
                    radioClass: 'iradio_flat-green'
                });
            },
        });
    }



    //Enable iCheck plugin for checkboxes
    //iCheck for checkbox and radio inputs
    $('.task-table input[type="checkbox"]').iCheck({
        checkboxClass: 'icheckbox_flat-green',
        radioClass: 'iradio_flat-green'
    });

    //Enable check and uncheck all functionality
    $(".checkbox-toggle").click(function () {
        var clicks = $(this).data('clicks');
        if (clicks) {
            //Uncheck all checkboxes
            $(".task-table input[type='checkbox']").iCheck("uncheck");
            $(".fa", this).removeClass("fa-check-square-o").addClass('fa-square-o');
        } else {
            //Check all checkboxes
            $(".task-table input[type='checkbox']").iCheck("check");
            $(".fa", this).removeClass("fa-square-o").addClass('fa-check-square-o');
        }
        $(this).data("clicks", !clicks);
    });

    //Handle starring for glyphicon and font awesome
    $(".task-star").click(function (e) {
        e.preventDefault();
        //detect type
        var $this = $(this).find("a > i");
        var glyph = $this.hasClass("glyphicon");
        var fa = $this.hasClass("fa");

        //Switch states
        if (glyph) {
            $this.toggleClass("glyphicon-star");
            $this.toggleClass("glyphicon-star-empty");
        }

        if (fa) {
            $this.toggleClass("fa-star");
            $this.toggleClass("fa-star-o");
        }
    });
}

$(function () {
    Init();
});