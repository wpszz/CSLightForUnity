2015-11-03 CSLight扩展版1.0.1

该版本是基于原版CSLight0.51Beta修改的，也是在项目开发过程中不断累积而成
下面介绍相对原版做出的主要修改。

下载地址：
https://github.com/wpszz/CSLightForUnity

原版地址：
https://github.com/lightszero/CSLightStudio

QQ交流群：135909946

支持功能：
1：支持函数重载或重写（取消了多接口继承，只能单类继承，多接口继承后面版本考虑）
   
2：支持get/set块属性，允许重载或重写

3：支持Unity协程（未完全支持，协程限制可以参考脚本文件CSMapTestCoroutine.cs，后面版本待改进）

4：支持位操作

5：支持typeof，前提是该类型是已注册的C#类型(不是CSLight脚本类型)

6: 脚本动态编译，只有引用到才编译

7: 支持protobuf-net（用法可以参考脚本文件CSMapTestProto.cs）

8：支持UnityEngine.Random.Range(0, 1)的写法（Random比较特殊，会与System的冲突，故特殊处理）


优化：
使用对象池优化部分频繁使用的对象，代码上优化的地方蛮多的，这里也不一一列举了，
尽量缩小CSLight脚本代码和C#代码上的差异性
脚本报错的调用堆栈信息优化
Token优化（删除了脚本打成AssetBundle后的调试信息，尽量在编辑器里调试脚本）
执行效率大家可以自己做下测试


其他：
1：只允许单个文件实现单个类，且类名和文件名一致

2：添加了内置注册类型，详情查看CLS_Environment.cs代码

3：脚本存放路径为Asset/CSLight/Editor/CSLogic，特意放在Editor下，借助强大的VS，编写脚本非常方便

4：添加CSLightMng，用以C#启动或访问CSLight脚本，具体用法参考Demo代码
   如果需要在编辑器里使用AssetBundle，请打开CSLightMng.cs的宏注释 #define EDITOR_FORCE_ASSET_BUNDLE

5：添加了一个去反射提高效率的转换工具，包括：
   CSLight/Editor/Tool/ToCSLightEditor.cs，入口文件，在Unity主界面的菜单栏上会生成相应的菜单
   CSLight/Editor/Res/ToCSLightTemplate.txt，模版文件
   CSLight/Editor/Res/ToCSLightRule.txt，转换规则(json格式，可以参考内部已有的规则)
   给函数或属性添加[EditorOnly]属性，可以在转换时过滤掉
   提示：你也可以不使用该转换工具，自己在ToCSLight.cs里手动注册类型

6：添加一个Unity5下脚本AssetBundle的打包方式，包括：
   CSLight/Editor/Tool/GenCSLightAssetBundle.cs，入口文件，在Unity主界面的菜单栏上会生成相应的菜单
   Unity4的自行修改打包方式

7：添加一个运行时的脚本命令窗口，包括：
   CSLight/Editor/Tool/WindowCommands.cs，入口文件，在Unity主界面的菜单栏上会生成相应的菜单
   默认收藏了Demo启动入口的脚本命令


引用其他库：
MiniJson
NGUI的BetterList
Protobuf-net
