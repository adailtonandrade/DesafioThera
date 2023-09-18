$(document).ready(function () { SetValidate(); });

function SetValidate() {
    function escapeAttributeValue(value) {
        return value.replace(/([!"#$%&'()*+,./:;<=>?@\[\\\]^`{|}~])/g, "\\$1");
    }
    $('form').each(function () {
        if ($.data(this, 'validator')) {
            var settngs = $.data(this, 'validator').settings;

            var oldErrorFunction = settngs.errorPlacement;
            var oldSucessFunction = settngs.success;

            settngs.errorPlacement = function (error, inputElement) {
                var container = $(this).find("[data-valmsg-for='" + escapeAttributeValue(inputElement[0].name) + "']");
                if (container) inputElement.closest(".form-group").addClass("has-error");
                oldErrorFunction(error, inputElement);
            };

            settngs.success = function (error) {
                var container = error.data("unobtrusiveContainer");
                if (container) container.closest(".form-group").removeClass("has-error");
                oldSucessFunction(error);
            };
        } else {
            console.warn("Form validator is not defined!");
        }
    });
}

function highLightError(element, errorClass) {
    if (element) {
        element = $(element);
        element.closest(".form-group").addClass("has-error");
        element.addClass(errorClass);
    }
}

function unhighLightError(element, errorClass) {
    if (element) {
        element = $(element);
        element.closest(".form-group").removeClass("has-error");
        element.removeClass(errorClass);
    }
}

function AddHasErrorInParent(errorList) {
    if (errorList) {
        errorList.forEach(function (name) {
            var myElement = $(document.getElementById(name));
            if (myElement) highLightError(myElement, 'input-validation-error');
        });
    }
}

function AddHasErrorInParentByName(errorList) {
    if (errorList) {
        errorList.forEach(function (name) {
            var myElement = $("input[name=\"" + name + "\"]");
            if (myElement) highLightError(myElement, 'input-validation-error');
        });
    }
}