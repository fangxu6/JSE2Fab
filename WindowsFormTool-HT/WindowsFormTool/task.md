# DPAT CSV 改造任务

- [x] 任务1：按新 CSV 表头 `TestNo,SiteNo,Bin,Time/mS,X,Y` 解析数据，并将 `Y` 后所有列识别为测试项（不再依赖 `Test Name` 标识行）。
- [x] 任务2：移除 DPAT 参数界面的上下限手工输入，改为运行时自动使用 CSV 内对应测试项的 `MIN/MAX` 作为上下限。
- [x] 任务3：确保 DPAT INK 的统计、越界判断与回写仅基于 `Bin=1` 的坐标数据执行。
- [x] 任务4：同步 OpenSpec 文档（`spec/proposal/design/tasks`）与当前实现，并统一为中文描述。
- [x] 任务5：针对指定样例（`STT_MWMN323PR0_8SITE_V1HT_CERHD8000-HT-7_20260205.csv` + `007.CERHD8000-HT-7`）完成调试，修复坐标系不一致导致的无法 INK 问题，并补充日志与文档。
