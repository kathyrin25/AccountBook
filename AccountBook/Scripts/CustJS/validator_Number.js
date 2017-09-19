$.validator.unobtrusive.adapters.addSingleVal("positiveinteger", "input");
$.validator.addMethod("positiveinteger", function (value, element, param) {
    if (value == false) {
        return true;
    }    
    if (Number(value) <= Number(param)) {
        return false;
    }
    else {
        return true;
    }
});