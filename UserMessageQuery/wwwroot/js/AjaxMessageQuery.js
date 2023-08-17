$("#form-submit").click(function (event) {
    event.preventDefault();

    let userName = $("#inputUserName").val();
    let message = $("#inputMessage").val();

    $.post("/Home/AddMessageToQuery", { userName, message }, function () {
        loadMessages();
    });

    function loadMessages() {
        $.get("/Home/GetAllMessages")
            .done(function (data) {
                $("#allMessageTableField").empty();
                $.each(data, function (index, message) {
                    $("#allMessageTableField").append(
                        '<tr>' +
                        '<th scope="row">' + (index + 1) + '</th>' +
                        '<td>' + message.id + '</td>' +
                        '<td>' + message.createTime + '</td>' +
                        '<td>' + message.description + '</td>' +
                        '</tr>'
                    );
                });
            });
    }
});

$("#form-submit-username").click(function (event) {
    event.preventDefault();

    let userName = $("#findUserName").val();

    $.get("/Home/GetMessagesByUserName", { userName })
        .done(function (data) {
            console.log(data);
            $("#userMessageTableField").empty();
            $("#findedUser").text(`Messages Users: ${userName}`);
            $.each(data, function (index, message) {
                $("#userMessageTableField").append(
                    '<tr>' +
                    '<th scope="row">' + (index + 1) + '</th>' +
                    '<td>' + message.id + '</td>' +
                    '<td>' + message.createTime + '</td>' +
                    '<td>' + message.description + '</td>' +
                    '</tr>'
                );
            });
        })
        .fail(function () {
            $("#findedUser").text(`User not found!`);
        });
});