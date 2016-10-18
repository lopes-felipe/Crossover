var register = function (baseURI, displayName, successCallback) {
    sendRequest(baseURI + "/register", "POST", { "display_name": displayName },
        function (data) {
            successCallback(data);
        });
}