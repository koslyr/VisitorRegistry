var sessionTimeoutWarning = SessionTimeout - 1;
var sTimeOutWarning = parseInt(sessionTimeoutWarning) * 60 * 1000;

var sTimeOutSession = parseInt(SessionTimeout) * 60 * 1000;

if (window.IsAuthenticated == 1) {
    setTimeout('SessionEndLogOff()', sTimeOutSession);
}

function SessionEndLogOff() {
    window.location = "/Account/LogOff";
}