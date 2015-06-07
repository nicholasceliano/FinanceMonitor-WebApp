var webAPI = {
    GeneralAccounts: {
        AllAccountsByUser: function (userID) {
            $.ajax({
                type: 'GET',
                url: 'api/generalAccounts/GetAllAccountsByUser/' + userID,
                contentType: 'application/json',
                dataType: 'json',
                success: function (data) {  
                    $('#lblAllAccountsByUser').text(JSON.stringify(data));
                }
            });
        },
        ValidateUserCredentials: function (userName, password) {
            var jsondata = {
                'userid': userName,
                'password': password
            };

            $.ajax({
                type: 'POST',
                data: JSON.stringify(jsondata),
                url: 'api/generalAccounts/ValidateUserCredentials',
                contentType: 'application/json',
                dataType: 'json',
                success: function (data) {
                    $('#lblValidateUserInfo').text(JSON.stringify(data));
                }
            });
        },
        LatestAmountByConnNameAccType: function (accID) {
            $.ajax({
                type: 'GET',
                url: 'api/generalAccounts/GetLatestAmountByConnNameAccType/' + accID,
                contentType: 'application/json',
                dataType: 'json',
                success: function (data) {
                    $('#lblLatestAmountByConnNameAccType').text(JSON.stringify(data));
                }
            });
        },
        AllLatestAmountsByUser: function (userID) {
            $.ajax({
                type: 'GET',
                url: 'api/generalAccounts/GetAllLatestAmountsByUser/' + userID,
                contentType: 'application/json',
                dataType: 'json',
                success: function (data) {
                    $('#lblAllLatestAmountsByUser').text(JSON.stringify(data));
                }
            });
        },
        UpdateAccountConnectionCredentials: function (accID, userName, password) {
            var jsondata = {
                'accID': accID,
                'userid': userName,
                'password': password
            };

            $.ajax({
                type: 'POST',
                data: JSON.stringify(jsondata),
                url: 'api/generalAccounts/UpdateAccountConnectionCredentials/',
                contentType: 'application/json',
                dataType: 'json',
                success: function (data) {
                    $('#lblUpdateAccountConnectionCredentials').text(JSON.stringify(data));
                }
            });
        },
        UpdateUserPassword: function (userID, password) {
            var jsondata = {
                'userID': userID,
                'password': password
            };

            $.ajax({
                type: 'POST',
                data: JSON.stringify(jsondata),
                url: 'api/generalAccounts/UpdateUserPassword/',
                contentType: 'application/json',
                dataType: 'json',
                success: function (data) {
                    $('#lblUpdateUserPassword').text(JSON.stringify(data));
                }
            });
        },
        CreateUser: function (userID, password, firstName, lastName) {
            var jsondata = {
                'userID': userID,
                'password': password,
                'firstName': firstName,
                'lastName' : lastName
            };

            $.ajax({
                type: 'POST',
                data: JSON.stringify(jsondata),
                url: 'api/generalAccounts/CreateUser/',
                contentType: 'application/json',
                dataType: 'json',
                success: function (data) {
                    $('#lblCreateUser').text(JSON.stringify(data));
                }
            });
        },
        DeleteUser: function (userID) {
            $.ajax({
                type: 'DELETE',
                url: 'api/generalAccounts/DeleteUser/' + userID,
                contentType: 'application/json',
                dataType: 'json',
                success: function (data) {
                    $('#lblDeleteUser').text(JSON.stringify(data));
                }
            });
        },
        CreateAccount: function (userID, connNameID, accountTypeID) {
            var jsondata = {
                'userID': userID,
                'connectionNameID': connNameID,
                'accountTypeID': accountTypeID
            };

            $.ajax({
                type: 'POST',
                data: JSON.stringify(jsondata),
                url: 'api/generalAccounts/CreateAccount/',
                contentType: 'application/json',
                dataType: 'json',
                success: function (data) {
                    $('#lblCreateAccount').text(JSON.stringify(data));
                }
            });
        },
        DeleteAccount: function (accID) {
            $.ajax({
                type: 'DELETE',
                url: 'api/generalAccounts/DeleteAccount/' + accID,
                contentType: 'application/json',
                dataType: 'json',
                success: function (data) {
                    $('#lblDeleteAccount').text(JSON.stringify(data));
                }
            });
        },
        GetAllPossibleAccounts: function () {
            $.ajax({
                type: 'GET',
                url: 'api/generalAccounts/GetAllPossibleAccounts/',
                contentType: 'application/json',
                dataType: 'json',
                success: function (data) {
                    $('#lblGetAllPossibleAccounts').text(JSON.stringify(data));
                }
            });
        }
    },
    Tests: {
        TestEmail: function () {
            $.ajax({
                type: 'GET',
                url: 'api/tests/test',
                contentType: 'application/json',
                dataType: 'json',
                success: function (data) {
                    
                }
            });
        },
        WebHookTest: function () {
            var jsondata = {
                'userID': 1,
                'connectionNameID': 2,
                'accountTypeID': 3
            };

            $.ajax({
                type: 'POST',
                data: JSON.stringify(jsondata),
                url: 'api/tests/WebHookTest/',
                contentType: 'application/json',
                dataType: 'json',
                success: function (data) {
                    $('#lblCreateAccount').text(JSON.stringify(data));
                }
            });
        }
    }
};