* 像素画工具 Aseprite

配合Unity2D，完成TileMap等工作，技巧：在slice得到Palette(调色板)时，若按照size slice，则最好在空白的地方点好点，防止空白处未被切割。这样在更新PNG图片时，不会导致Palette混乱。

* Profiler 发现动画较耗时，则默认关闭动画，需要时再enable，而不是给一个默认空状态（因为还是耗时）

* 2d extra 的 Prefab brush 有bug，可能在同一tile刷上多个Prefab


