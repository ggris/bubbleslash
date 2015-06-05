var config = {
    width: Math.min($(document).width() - 100, 900), 
    height: Math.min($(document).height() - 100, 600),
    params: { enableDebugging:"0" }

};
var u = new UnityObject2(config);

function loadGame() {

    var version = window.location.search.substring(1);
    document.getElementById("Version").innerHTML = version;
    var $missingScreen = jQuery("#unityPlayer").find(".missing");
    var $brokenScreen = jQuery("#unityPlayer").find(".broken");
    $missingScreen.hide();
    $brokenScreen.hide();

    u.observeProgress(function (progress) {
        switch(progress.pluginStatus) {
            case "broken":
                $brokenScreen.find("a").click(function (e) {
                    e.stopPropagation();
                    e.preventDefault();
                    u.installPlugin();
                    return false;
                });
                $brokenScreen.show();
                break;
            case "missing":
                $missingScreen.find("a").click(function (e) {
                    e.stopPropagation();
                    e.preventDefault();
                    u.installPlugin();
                    return false;
                });
                $missingScreen.show();
                break;
            case "installed":
                $missingScreen.remove();
                break;
            case "first":
                break;
        }
    });
    u.initPlugin(jQuery("#unityPlayer")[0], "embedded/bubbleslash-"+version+".unity3d");
}
