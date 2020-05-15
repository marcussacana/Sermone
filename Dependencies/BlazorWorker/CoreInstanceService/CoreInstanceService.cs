﻿using System;
using System.Threading.Tasks;
using BlazorWorker.Core.SimpleInstanceService;
using BlazorWorker.WorkerCore.SimpleInstanceService;


namespace BlazorWorker.Core.CoreInstanceService
{
    using TargetType = BlazorWorker.WorkerCore.SimpleInstanceService.SimpleInstanceService;
    internal class CoreInstanceService : ICoreInstanceService
    {
        private static long sourceId;
        private readonly SimpleInstanceServiceProxy simpleInstanceServiceProxy;

        public CoreInstanceService(SimpleInstanceServiceProxy simpleInstanceServiceProxy)
        {
            this.simpleInstanceServiceProxy = simpleInstanceServiceProxy;
        }
        public Task<IInstanceHandle> CreateInstance<T>()
        {
            return CreateInstance(typeof(T));
        }
        public async Task<IInstanceHandle> CreateInstance(Type t)
        {
            var id = ++sourceId;
            if (!this.simpleInstanceServiceProxy.IsInitialized)
            {
                await this.simpleInstanceServiceProxy.InitializeAsync(new WorkerInitOptions()
                {
                    DependentAssemblyFilenames = new[] { $"{t.Assembly.GetName().Name}.dll" },
                    InitEndPoint = $"[{typeof(TargetType).Assembly.GetName().Name}]{typeof(TargetType).FullName}:{nameof(TargetType.Init)}"
            });
            }
            var initResult = await this.simpleInstanceServiceProxy.InitInstance(
                new InitInstanceRequest()
                {
                    Id = id,
                    TypeName = t.FullName,
                    AssemblyName = t.Assembly.GetName().FullName
                });

            if (!initResult.IsSuccess)
            { 
                throw new WorkerInstanceInitializeException(initResult.ExceptionMessage, initResult.FullExceptionString);  
            }

            return new CoreInstanceHandle(async () => await OnDispose(id));
        }

        private async Task OnDispose(long id)
        {
            var result = await this.simpleInstanceServiceProxy.DisposeInstance(
                            new DisposeInstanceRequest() { InstanceId = id });
            if (result.IsSuccess)
            {
                return;
            }

            throw new WorkerInstanceDisposeException(result.ExceptionMessage, result.FullExceptionString);
        }
    }
}
