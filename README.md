# Player_2D
角色基本移动，跳跃，跳墙，冲刺残影，角色基本攻击功能，AI敌人，一些shader与特效，背包系统，对话系统

其中使用有限状态机添加的敌人，完整的状态转换图与大体上的UML图可在附件中的照片查看。

可根据状态转换图，根据UML类图，添加新状态的步骤如下：

1，创建新的状态类（继承于State）

2，创建状态数据类（继承于ScriptableObject）

3，创建敌人特定状态类（继承于上面创建的状态类）

4，在实例敌人（对应UML图中的Enemy类）中声明，调用该状态

5，设置对应动画

添加的shader是使用ShaderGraph创建的，其中创建了雨，水面，渐变的天空的材质。

该项目的功能目前还在学习扩展，未来希望添加武器系统，添加基础的UI让其成为完整的demo