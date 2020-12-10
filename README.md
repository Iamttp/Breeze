# Breeze 客户端

* 像素画工具 Aseprite

配合Unity2D，完成TileMap等工作，技巧：在slice得到Palette(调色板)时，若按照size slice，则最好在空白的地方点好点，防止空白处未被切割。这样在更新PNG图片时，不会导致Palette混乱。

* Profiler 发现动画较耗时，则默认关闭动画，需要时再enable，而不是给一个默认空状态（因为还是耗时）

* 2d extra 的 Prefab brush 有bug，可能在同一tile刷上多个Prefab

* 血泪教训，网络相关处理函数一定放在FixedUpdate。否则Update调用时间不固定！

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

