FROM node:18 AS build

WORKDIR /app

COPY package*.json ./
RUN npm install
COPY . ./
RUN npm run build --prod

FROM nginx:alpine

COPY --from=build /app/dist/feature-proj/browser /usr/share/nginx/html

COPY nginx.conf /etc/nginx/nginx.conf

EXPOSE 80
