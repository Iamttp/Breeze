# Breeze 客户端

* 像素画工具 Aseprite

配合Unity2D，完成TileMap等工作，技巧：在slice得到Palette(调色板)时，若按照size slice，则最好在空白的地方点好点，防止空白处未被切割。这样在更新PNG图片时，不会导致Palette混乱。

* Profiler 发现动画较耗时，则默认关闭动画，需要时再enable，而不是给一个默认空状态（因为还是耗时）

* 2d extra 的 Prefab brush 有bug，可能在同一tile刷上多个Prefab

* 血泪教训，网络相关处理函数一定放在FixedUpdate。否则Update调用时间不固定！

* 服务器传输数据时人物数据逻辑考虑：

方法1： 当设置SpeedVal = 0时，不靠下面的语句更新位置，靠transform强制更新位置，出现人物卡顿的情况，分析数据传输时有一部分没有处理（应该不是丢包，毕竟tcp）。这样会在人物移动结束时，可能来不及更新速度，导致人物位置回退。

```csharp
rg.MovePosition(rg.position + MoveVec * SpeedVal * Time.fixedDeltaTime);
```

方法2： 当不设置SpeedVal = 0时，人物移动断断续续，原因同上。

* 服务器传输数据时人物数据逻辑考虑(New)：

采用方法1，测试发现为多个人物时，需要向单个人物同时发送其他所有人的数据造成服务器处理不及时，采用方法2，同时设置人物在诸如移动、攻击等特殊条件下在发送数据。同时设置float类型为string类型并保留2位小数，使用json格式传输（考虑 protobuf)，在单机多客户端上效果较不错。

---

## MsgId 约定

| 消息id  | 消息功能  | 服务器端函数 | 数据结构 | 
| ----   | ----     |  ---- | ---- |
| 1      |   服务器端告知客户端玩家的ID（上线）                   |   syncPid  | |
| 2      |   服务器端告知客户端玩家的pos（上线）                  |   syncPos  | |
| 3      |   服务器端广播位置信息，接收201后调用                  |   SyncOtherPos | |
| 4      |   服务器端告知客户端玩家的ID（下线）                   |   syncUnPid  | |
| 201    |   客户端发送移动请求和位置信息                        |   moveRouter.Handle | |

---

WASD    移动
Shift   移动加速
R       建造物品时旋转
Space   攻击
Tab     切换人物
F       拾取
123     背包换页

---

