$(function () {
    $("#show-add-contributor-modal").on('click', function () {
        $("#modal-title").text("Add Contributor");
        $("#contributor-modal").modal();
        $("#form").attr('action', "/home/addcontributor");
    });
    $(".table").find('.deposit').on('click', function () {
        var id = $(this).data('id');
        $("#contributor-id").val(id);
        $("#deposit-modal").modal();
    });
    $(".table").find(".edit").on('click', function () {
        var button = $(this);
        $("#modal-title").text("Edit Contributor");
        $("#contributor-modal").modal();
        $("#first-name").val(button.data('first-name'));
        $("#last-name").val(button.data('last-name'));
        $("#cell-number").val(button.data('cell-number'));
        $("#id").val(button.data('id'));
        $("#initial-deposit").hide();
        $("#form").attr('action', "/home/editcontributor");
        var alwaysInclude = button.data('always-include');
        if (alwaysInclude) {
            $("#always-include").prop('checked', true);
        }
        else {
            $("#always-include").prop('checked', false);
        }
        var month = button.data('month');
        var day = button.data('day');
        var year = button.data('year');
        var date = year + "-" + month + "-" + day;
        $("#date-created").val(date);
        //$("#date-created").datepicker();
        //$("#date-created").datepicker('setDate', button.val());

        //$("#date-created").val(button.data('date-created'));
        //$("#date-created").datepicker();
        //$("#date-created").datepicker('setDate', button.val());
    });

    $("#search-input").on('keyup', function () {
        var input = $("#search-input").val().toLowerCase();
        $('tr').each(function ( index ) {
            var contains = $(this).children().eq(1).text().toLowerCase().indexOf(input);
            console.log(contains);
            if (contains > -1){
                $(this).show();
            }
            else {
                $(this).hide();
            }
        });
    });
   
});