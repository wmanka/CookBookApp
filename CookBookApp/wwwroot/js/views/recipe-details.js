$(function () {

    $('#emailForm').hide();

    $('#favouriteBtn').click(function () {
        var data = $.parseJSON($(this).attr('data-button'));
        var recipeId = data.recipeId;
        var userId = data.userId;
        var numberOfLikes = data.numberOfLikes;

        AddToFavourite(userId, recipeId, numberOfLikes);
    });

    $('#emailRecipe').click(function () {
        var data = $.parseJSON($(this).attr('data-button'));
        var recipeId = data.recipeId;
        var email = $('#email').val();

        var vm = new Object();
        vm.RecipeId = recipeId;
        vm.Email = email;

        $.ajax({
            url: "/api/recipeEmailSender",
            method: "POST",
            data: vm,
            success: function () {
                toastr.success("Recipe has been sent");
                $('#emailForm').toggle("slow");
                $('#emailForm input[type=text]').val("");
            },
            error: function () {
                toastr.error("Something went wrong");
            }
        });
    });

    $('#toggleEmail').click(function () {
        $('#emailForm').toggle("slow");
    });

    $('#shoppingList').click(function () {
        toastr.info("Creating shopping list");
        var data = $.parseJSON($(this).attr('data-button'));
        var recipeId = data.recipeId;

        $.ajax({
            url: "/api/googleTaskLists?id=" + recipeId,
            method: "POST",
            data: recipeId,
            success: function () {
                toastr.remove();
                toastr.success('Items successfully added to shopping list');
            },
            error: function (response) {
                toastr.remove();
                if (response.status == "409") {
                    toastr.warning("You've already created shopping list for this recipe");
                }
                else {
                    toastr.error('Something went wrong');
                }
            }
        });
    });

    $('#commentSubmit').click(function () {
        var data = $.parseJSON($(this).attr('data-button'));
        var recipeId = data.recipeId;
        var userId = data.userId;
        var content = $('#commentContent').val();

        var vm = new Object();
        vm.RecipeId = recipeId;
        vm.UserId = userId;
        vm.Content = content;

        $.ajax({
            url: "/api/comments",
            method: "POST",
            data: vm,
            success: function (response) {
                toastr.success("Comment has been successfully added");
                $('#commentsList').prepend('<div class="d-flex w-100 justify-content-between"><h5 class="mb-1">' +
                    response.content +'</h5 ><small>' + response.createdAt + '</small></div><small>' + response.userId + '</small>'
                );
            },
            error: function () {
                toastr.error("Something went wrong");
            }
        });
    });

    function RemoveFromFavourite(userId, recipeId, numberOfLikes) {
        $.ajax({
            url: "/api/favouriterecipes/" + userId + "/" + recipeId,
            method: "DELETE",
            success: function () {
                toastr.success('Recipe successfully removed from favourite');
                if (numberOfLikes > 0) {
                    numberOfLikes -= 1;
                    $('[name="isFavourited"]').html(numberOfLikes + '<i class="far fa-heart"></i>');
                }
                else
                    $('[name="isFavourited"]').html('0 <i class="far fa-heart"></i>');
            }
        });
    }

    function AddToFavourite(userId, recipeId, numberOfLikes) {
        $.ajax({
            url: "/api/favouriterecipes/",
            method: "POST",
            data: { UserId: userId, RecipeId: recipeId },
            success: function () {
                toastr.success('Recipe successfully added to favourite');
                numberOfLikes = + 1;
                $('[name="isFavourited"]').html(+numberOfLikes + '<i class="fas fa-heart"></i>');
            },
            error: function () {
                RemoveFromFavourite(userId, recipeId, numberOfLikes);
            }
        });
    }
});