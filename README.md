# LeanEngineApp

.Net LeanCloud 云引擎 SDK 及脚手架工程

## 常见用法

### 云函数

```csharp
// 异步云函数
[LCEngineFunction("Hello")]
public static Task<string> Hello(LCCloudFunctionRequest request) {
  string msg = $"hello, {request.Params["name"]}";
  Console.WriteLine(msg);
  return Task.FromResult(msg);
}

// 同步云函数
[LCEngineFunction("Hi")]
public static string Hi(LCCloudFunctionRequest request) {
  return $"hi, {request.Params["name"]}";
}

// 返回数组类型 RPC
[LCEngineFunction("GetTodos")]
public static async Task<ReadOnlyCollection<LCObject>> GetTodos(LCCloudFunctionRequest request) {
  LCQuery<LCObject> query = new LCQuery<LCObject>("Todo");
  ReadOnlyCollection<LCObject> todos = await query.Find();
  return todos;
}

// 返回字典类型 RPC
[LCEngineFunction("GetTodoMap")]
public static async Task<Dictionary<string, LCObject>> GetTodoMap(LCCloudFunctionRequest request) {
  LCQuery<LCObject> query = new LCQuery<LCObject>("Todo");
  ReadOnlyCollection<LCObject> todos = await query.Find();
  return todos.ToDictionary(todo => todo.ObjectId);
}

// 返回 LCObject 类型 RPC
[LCEngineFunction("GetTodo")]
public static async Task<LCObject> GetTodo(LCCloudFunctionRequest request) {
  LCQuery<LCObject> query = new LCQuery<LCObject>("Todo");
  return await query.Get("603f11d44a28a709aaa0b077");
}
```

### 对象 Hook

```csharp
// Todo 对象保存前
[LCEngineObjectHook("Todo", LCEngineObjectHookType.BeforeSave)]
public static Task<LCObject> BeforeSaveTodo(LCClassHookRequest request) {
  LCObject todo = request.Object;
  todo["content"] = "xxx";
  return Task.FromResult(todo);
}

// Todo 对象保存后
[LCEngineObjectHook("Todo", LCEngineObjectHookType.AfterSave)]
public static void AfterSaveTodo(LCClassHookRequest request) {

}

// Todo 对象更新前
[LCEngineObjectHook("Todo", LCEngineObjectHookType.BeforeUpdate)]
public static Task<LCObject> BeforeUpdateTodo(LCClassHookRequest request) {
  return Task.FromResult(request.Object);
}

// Todo 对象更新后
[LCEngineObjectHook("Todo", LCEngineObjectHookType.AfterUpdate)]
public static void AfterUpdateTodo(LCClassHookRequest request) {

}

// Todo 对象删除前
[LCEngineObjectHook("Todo", LCEngineObjectHookType.BeforeDelete)]
public static Task<LCObject> BeforeDeleteTodo(LCClassHookRequest request) {
  throw new Exception("Can't delete.");
}

// Todo 对象删除后
[LCEngineObjectHook("Todo", LCEngineObjectHookType.AfterDelete)]
public static void AfterDelete(LCClassHookRequest request) {
  throw new Exception("Can't delete.");
}
```

### 用户 Hook

```csharp
// 短信验证后
[LCEngineUserHook(LCEngineUserHookType.SMS)]
public static void OnVerifiedSMS(LCUserHookRequest request) {
  Console.WriteLine($"{request.CurrentUser.ObjectId} verified mobile.");
}

// 邮箱验证后
[LCEngineUserHook(LCEngineUserHookType.Email)]
public static void OnVerifiedEmail(LCUserHookRequest request) {
  Console.WriteLine($"{request.CurrentUser.ObjectId} verified email.");
}

// 登录后
[LCEngineUserHook(LCEngineUserHookType.OnLogin)]
public static Task OnLogin(LCUserHookRequest request) {
  Console.WriteLine($"{request.CurrentUser.ObjectId} login.");
  return Task.FromResult(request.CurrentUser);
}
```

### 即时通讯 Hook

```csharp
// 用户上线
[LCEngineRealtimeHook(LCEngineRealtimeHookType.ClientOnline)]
public static Task OnClientOnline(LCCloudFunctionRequest request) {
  Console.WriteLine($"{request.Params["peerId"]} online.");
  return Task.CompletedTask;
}

// 用户下线
[LCEngineRealtimeHook(LCEngineRealtimeHookType.ClientOffline)]
public static Task OnClientOffline(LCCloudFunctionRequest request) {
  Console.WriteLine($"{request.Params["peerId"]} offline.");
  return Task.CompletedTask;
}

// 消息发出（到达 LeanCloud 服务器）
[LCEngineRealtimeHook(LCEngineRealtimeHookType.MessageSent)]
public static Task<object> OnMessageSent(LCCloudFunctionRequest request) {
  Dictionary<string, object> data = request.Params;
  data["content"] = (data["content"] as string).Replace("hello", "hi");
  Console.WriteLine($"{data["fromPeer"]} : {data["content"]}");
  return Task.FromResult<object>(data);
}

// 消息收到后
[LCEngineRealtimeHook(LCEngineRealtimeHookType.MessageReceived)]
public static Task<object> OnMessageReceived(LCCloudFunctionRequest request) {
  Dictionary<string, object> data = request.Params;
  data["content"] = (data["content"] as string).Replace("hello", "hi");
  Console.WriteLine($"{data["fromPeer"]} : {data["content"]}");
  return Task.FromResult<object>(new Dictionary<string, object> {
    { "content", data["content"] }
  });
}
```

