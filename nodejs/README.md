# 深澜软件网络认证辅助工具

## 使用方法

### 配置深澜认证服务主机地址

- 修改 `src/config.ts` 中的 `BASE_URL`

### 配置深澜账号信息

- 设置深澜账号环境变量
- 环境变量名称默认为： `SRUN_ACCOUNT`，可在 `src/config.ts` 修改所使用的环境变量名称
- 环境变量内容为：`用户名:密码`

### 安装依赖

```shell
npm install
```

### 运行

```shell
npm run start
```

### 打包构建

```shell
npm run build
```

## 目录结构

```
├─dist - 打包输出目录
└─src - 项目源码
   ├───api - API 目录
   ├───interfaces - TypeScript 接口
   ├───srun - 深澜软件相关
   └───utils - 工具库
```

