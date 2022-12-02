FROM nginx:stable-alpine AS base
WORKDIR /app
EXPOSE 80

FROM node:lts-alpine3.15 AS build
WORKDIR /src
COPY web/ .
WORKDIR "/src"
# RUN npm config set registry https://registry.npm.taobao.org
RUN npm i
RUN npm run build

FROM base AS file-web
WORKDIR /app
COPY --from=build /src/dist /wwwroot/
COPY web/conf.d/ /etc/nginx/conf.d/