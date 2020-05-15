window.BlazorWorker = function () {
    
    const workers = {};
    const disposeWorker = function (workerId) {

        const worker = workers[workerId];
        if (worker && worker.terminate) {
            worker.terminate();
            delete workers[workerId];
        }
    };

    const workerDef = function () {
        if (typeof (self.window) == 'undefined')
            self.window = self;
        eval("var DotNet;!function(n){self.DotNet=n;var t=[],r={},e={},o=1,i=null;function u(n){t.push(n)}function c(n,t){for(var r=[],e=2;e<arguments.length;e++)r[e-2]=arguments[e];return f(n,t,null,r)}function s(n,t,r,e){var o=a();if(o.invokeDotNetFromJS){var i=JSON.stringify(e,w),u=o.invokeDotNetFromJS(n,t,r,i);return u?h(u):null}throw new Error(\"The current dispatcher does not support synchronous calls from JS to .NET. Use invokeMethodAsync instead.\")}function f(n,t,e,i){var u=o++,c=new Promise(function(n,t){r[u]={resolve:n,reject:t}});try{var s=JSON.stringify(i,w);a().beginInvokeDotNetFromJS(u,n,t,e,s)}catch(n){l(u,!1,n)}return c}function a(){if(null!==i)return i;throw new Error(\"No .NET call dispatcher has been set.\")}function l(n,t,e){if(!r.hasOwnProperty(n))throw new Error(\"There is no pending async call with ID \"+n+\".\");var o=r[n];delete r[n],t?o.resolve(e):o.reject(e)}function h(n){return n?JSON.parse(n,function(n,r){return t.reduce(function(t,r){return r(n,t)},r)}):null}function v(n){return n instanceof Error?n.message+\"\\n\"+n.stack:n?n.toString():\"null\"}function p(n){if(e.hasOwnProperty(n))return e[n];var t=window,r=\"window\";if(n.split(\".\").forEach(function(n){if(!(n in t))throw new Error(\"Could not find \'\"+n+\"\' in \'\"+r+\"\'.\");t=t[n],r+=\".\"+n}),t instanceof Function)return t;throw new Error(\"The value \'\"+r+\"\' is not a function.\")}n.attachDispatcher=function(n){i=n},n.attachReviver=u,n.invokeMethod=function(n,t){for(var r=[],e=2;e<arguments.length;e++)r[e-2]=arguments[e];return s(n,t,null,r)},n.invokeMethodAsync=c,n.jsCallDispatcher={findJSFunction:p,invokeJSFromDotNet:function(n,t){var r=p(n).apply(null,h(t));return null==r?null:JSON.stringify(r,w)},beginInvokeJSFromDotNet:function(n,t,r){var e=new Promise(function(n){n(p(t).apply(null,h(r)))});n&&e.then(function(t){return a().beginInvokeDotNetFromJS(0,\"Microsoft.JSInterop\",\"DotNetDispatcher.EndInvoke\",null,JSON.stringify([n,!0,t],w))},function(t){return a().beginInvokeDotNetFromJS(0,\"Microsoft.JSInterop\",\"DotNetDispatcher.EndInvoke\",null,JSON.stringify([n,!1,v(t)]))})},endInvokeDotNetFromJS:function(n,t,r){var e=t?r:new Error(r);l(parseInt(n),t,e)}};var d=function(){function n(n){this._id=n}return n.prototype.invokeMethod=function(n){for(var t=[],r=1;r<arguments.length;r++)t[r-1]=arguments[r];return s(null,n,this._id,t)},n.prototype.invokeMethodAsync=function(n){for(var t=[],r=1;r<arguments.length;r++)t[r-1]=arguments[r];return f(null,n,this._id,t)},n.prototype.dispose=function(){c(\"Microsoft.JSInterop\",\"DotNetDispatcher.ReleaseDotNetObject\",this._id).catch(function(n){return console.error(n)})},n.prototype.serializeAsArg=function(){return\"__dotNetObject:\"+this._id},n}(),N=/^__dotNetObject\\:(\\d+)$/;function w(n,t){return t instanceof d?t.serializeAsArg():t}u(function(n,t){if(\"string\"==typeof t){var r=t.match(N);if(r)return new d(parseInt(r[1]))}return t})}(DotNet||(DotNet={}));");
        const initConf = JSON.parse('$initConf$');
        const onReady = () => {
            const messageHandler =
                Module.mono_bind_static_method(initConf.MessageEndPoint);
            // Future messages goes directly to the message handler
            self.onmessage = msg => {
                messageHandler(msg.data);
            };

            if (!initConf.InitEndPoint) {
                return;
            }

            try {
                Module.mono_call_static_method(initConf.InitEndPoint, []);
            } catch (e) {
                console.error(`Init method ${initConf.InitEndPoint} failed`, e);
                throw e;
            }
        };

        const onError = (err) => {
            console.error(err);
        };

        function asyncLoad(url, reponseType) {
            return new Promise((resolve, reject) => {
                const xhr = new XMLHttpRequest();
                const arrayBufferType = 'arraybuffer';
                xhr.open('GET', url, /* async: */ true);
                xhr.responseType = reponseType || arrayBufferType;
                xhr.onload = function xhr_onload() {
                    if (xhr.status == 200 || xhr.status == 0 && xhr.response) {
                        if (this.responseType === arrayBufferType) {
                            const asm = new Uint8Array(xhr.response);
                            resolve(asm);
                        } else {
                            resolve(xhr.response);
                        }
                    } else {
                        reject(xhr);
                    }
                };
                xhr.onerror = reject;
                xhr.send(undefined);
            });
        }
        
        var config = {};
        var Module = {};
        
        const wasmBinaryFile = `${initConf.appRoot}/_framework/wasm/dotnet.wasm`;
        const suppressMessages = ['DEBUGGING ENABLED'];
        const appBinDirName = 'appBinDir';

        Module.print = line => (suppressMessages.indexOf(line) < 0 && console.log(`WASM-WORKER: ${line}`));

        Module.printErr = line => {
            console.error(`WASM-WORKER: ${line}`);
            showErrorNotification();
        };
        Module.preRun = [];
        Module.postRun = [];
        Module.preloadPlugins = [];

        Module.locateFile = fileName => {
            switch (fileName) {
                case 'dotnet.wasm': return wasmBinaryFile;
                default: return fileName;
            }
        };

        Module.preRun.push(() => {
            const mono_wasm_add_assembly = Module.cwrap('mono_wasm_add_assembly', null, [
                'string',
                'number',
                'number',
            ]);

            mono_string_get_utf8 = Module.cwrap('mono_wasm_string_get_utf8', 'number', ['number']);

            MONO.loaded_files = [];
            var baseUrl = `${initConf.appRoot}/${initConf.deploy_prefix}`;
            initConf.DependentAssemblyFilenames.forEach(url => {
                
                const runDependencyId = `blazor:${url}`;
                addRunDependency(runDependencyId);
                asyncLoad(baseUrl+'/'+ url).then(
                    data => {
                        const heapAddress = Module._malloc(data.length);
                        const heapMemory = new Uint8Array(Module.HEAPU8.buffer, heapAddress, data.length);
                        heapMemory.set(data);
                        mono_wasm_add_assembly(url, heapAddress, data.length);
                        MONO.loaded_files.push(url);
                        removeRunDependency(runDependencyId);
                    },
                    errorInfo => {
                        const isPdb404 = errorInfo instanceof XMLHttpRequest
                            && errorInfo.status === 404
                            && url.match(/\.pdb$/);
                        if (!isPdb404) {
                            onError(errorInfo);
                        }
                        removeRunDependency(runDependencyId);
                    }
                );
            });
        });

        Module.postRun.push(() => {
            MONO.mono_wasm_setenv("MONO_URI_DOTNETRELATIVEORABSOLUTE", "true");
            const load_runtime = Module.cwrap('mono_wasm_load_runtime', null, ['string', 'number']);
            load_runtime(appBinDirName, 0);
            MONO.mono_wasm_runtime_is_ready = true;
            onReady();
        });

        config.file_list = [];
        
        global = globalThis;
        self.Module = Module;

        //TODO: This call could/should be session cached. But will the built-in blazor fetch service worker override 
        // (PWA et al) do this already if configured ?
        asyncLoad(`${initConf.appRoot}/${initConf.blazorBoot}`, 'json')
            .then(blazorboot => {
                let dotnetjsfilename = '';
                const runttimeSection = blazorboot.resources.runtime;
                for (var p in runttimeSection) {
                    if (Object.prototype.hasOwnProperty.call(runttimeSection, p) && p.endsWith('.js')) {
                        dotnetjsfilename = p;
                    }
                }

                if (dotnetjsfilename === '') {
                    throw 'BlazorWorker: Unable to locate dotnetjs file in blazor boot config.';
                }

                self.importScripts(`${initConf.appRoot}/${initConf.wasmRoot}/${dotnetjsfilename}`);
            
            }, errorInfo => onError(errorInfo));
    };

    const inlineWorker = `self.onmessage = ${workerDef}()`; 

    const initWorker = function (id, callbackInstance, initOptions) {
        let appRoot = (document.getElementsByTagName('base')[0] || { href: window.location.origin }).href || "";
        if (appRoot.endsWith("/")) {
            appRoot = appRoot.substr(0, appRoot.length - 1);
        }

        const initConf = {
            appRoot: appRoot,
            DependentAssemblyFilenames: initOptions.dependentAssemblyFilenames,
            deploy_prefix: "_framework/_bin",
            MessageEndPoint: initOptions.messageEndPoint,
            InitEndPoint: initOptions.initEndPoint,
            wasmRoot: "_framework/wasm",
            blazorBoot: "_framework/blazor.boot.json",
            debug: initOptions.debug
        };

        // Initialize worker
        const renderedConfig = JSON.stringify(initConf).replace('$appRoot$', appRoot);
        const renderedInlineWorker = inlineWorker.replace('$initConf$', renderedConfig);
        window.URL = window.URL || window.webkitURL;
        const blob = new Blob([renderedInlineWorker], { type: 'application/javascript' });
        const worker = new Worker(URL.createObjectURL(blob));
        workers[id] = worker;

        worker.onmessage = function (ev) {
            if (initOptions.debug) {
                console.debug(`BlazorWorker.js:worker[${id}]->blazor`, initOptions.callbackMethod, ev.data);
            }
            callbackInstance.invokeMethod(initOptions.callbackMethod, ev.data);
        };
    };

    const postMessage = function (workerId, message) {
        workers[workerId].postMessage(message);
    };

    return {
        disposeWorker,
        initWorker,
        postMessage
    };
}();