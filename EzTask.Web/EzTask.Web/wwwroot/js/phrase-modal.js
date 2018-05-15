$(function () {

    function SetDeleteModalValue(projectId, phraseId) {
        $('#phrase-modal .project-id').val(projectId);
        $('#delete-modal .phrase-id').val(phraseId);
    }

    $(".btn-addnew-phrase").click(function () {
        var projectId = $('.project-list').val();
        SetDeleteModalValue(projectId, 0);
        SetModalTitle("phrase-modal", "Add new phrase");
        ShowModal('phrase-modal');
    });

});