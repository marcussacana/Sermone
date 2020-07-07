window.onclick = function(e) {
    if (!e.target.matches('.dropbtn')) {
        var DropDowns = document.getElementsByClassName('dropdown-content');
        for (var i = 0; i < DropDowns.length; i++) {
            var Dropdown = DropDowns[i];
            if (Dropdown.classList.contains('show')) {
                Dropdown.classList.remove('show');
            }
        }
  }
}

window.SetTitle = (title) => {
    document.title = title;
}

window.EnsureItemVisible = (item) => {
    var elm = document.getElementById("CK" + item).parentElement;
    elm.scrollIntoView({ block: 'center', behavior: 'smooth' });
}

window.FocusElement = (elm) => {
    elm.focus();
}

window.EnableTooltip = () => {
    try {
        $('tooltip').tooltip();
        $('*[data-toggle=\"tooltip\"]').tooltip();
    } catch { }
}

window.DestroyTooltip = () => {
    try {
        $('div[class*="tooltip fade"]').remove();
    }catch { }
}

window.GetBaseDirectory = () => {
    var Elm = document.getElementsByTagName("base")[0];
    return Elm.getAttribute("href");
}

function ShowDropDown(e) {
    e.classList.toggle("show");
}

function OpenFile(id) {
    var elm = document.getElementById(id);
    elm.value = null;
    elm.click();
}

async function Initialize() {
    if (typeof (ReadyFlag) == "undefined")
        return;

    if (Interval !== null) {
        clearInterval(Interval);
        Interval = null;
    }

    await DotNet.invokeMethodAsync('Sermone', 'Initialize');
    setTimeout(Initialize, 10);
}

async function Request(Method, Url, Data) {
    try {
        return await new Promise(function(resolve, reject) {
                var xhr = new XMLHttpRequest();
                xhr.open(Method, Url);
                xhr.onload = function() {
                    if (this.status >= 200 && this.status < 300) {
                        resolve(xhr.response);
                    } else {
                        reject({
                            status: this.status,
                            statusText: xhr.statusText
                        });
                    }
                };
                xhr.onerror = function() {
                    reject({
                        status: this.status,
                        statusText: xhr.statusText
                    });
                };
                xhr.send(Data);
            });
    } catch {
        return await fetch(Url, {
            "body": Data,
            "method": Method,
            "mode": "no-cors",
            "credentials": "omit"
        });
    }
}

var Interval = setInterval(Initialize, 100);