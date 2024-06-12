FROM node:20 AS build

ARG BACKEND_API=halma-web-api:8080
ENV VITE_BACKEND_API=$BACKEND_API

WORKDIR /app

COPY package*.json ./

RUN npm install

COPY . .

RUN npm run build

FROM nginx:alpine

COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80

CMD ["nginx", "-g", "daemon off;"]