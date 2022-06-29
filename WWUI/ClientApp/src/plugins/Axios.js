import axios from "axios"
import { baseUrl } from "../api/ApiConfig";
import { notification, message } from "antd"

axios.defaults.baseURL = baseUrl //测试端口
axios.defaults.headers.post["Content-Type"] = "application/json;charset=UTF-8";
axios.defaults.headers.put["Content-Type"] = "application/json;charset=UTF-8";

//设置超时
axios.defaults.timeout = 10000;
//token拦截器
axios.interceptors.request.use(config => {
	const token = localStorage.getItem("token");
	if (token) {
		config.headers.Authorization = "Bearer " + token;
	}
	return config;
})

axios.interceptors.request.use(
	config => {
		return config;
	},
	error => {
		return Promise.reject(error);
	}
);

axios.interceptors.response.use(
	response => {
		if (response.status === 200) {
			if (response.data.code !== 200) {
				notification.error({
					message: response.data.msg,
					placement: "bottomRight"
				});
			}
			return Promise.resolve(response);
		} else {
			return Promise.reject(response);
		}
	},
	error => {
		//localStorage.removeItem("token")
		notification.error({
			message: "网络请求错误",
			description: error.message,
			placement: "bottomRight"
		})
		// setTimeout(() => {
		// 	window.location.href = "/login"
		// },1000)

	}
);
const Axios = {
	post({ url, data, params }) {
		return new Promise((resolve, reject) => {
			axios({
				method: 'post',
				url,
				data: data,
				params: params
			})
				.then(res => {
					resolve(res.data)
				})
				.catch(err => {
					reject(err)
				});
		})
	},

	get({ url, params }) {
		return new Promise((resolve, reject) => {
			axios({
				method: 'get',
				url,
				params: params,
			})
				.then(res => {
					resolve(res.data)
				})
				.catch(err => {
					reject(err)
				})
		})
	},

	put({ url, data, params }) {
		return new Promise((resolve, reject) => {
			axios({
				method: 'put',
				url,
				data: data,
				params: params
			})
				.then(res => {
					resolve(res.data)
				})
				.catch(err => {
					reject(err)
				})
		})
	},

	delete({ url, params }) {
		return new Promise((resolve, reject) => {
			axios({
				method: 'delete',
				url,
				params: params,
			})
				.then(res => {
					resolve(res.data)
				})
				.catch(err => {
					reject(err)
				})
		})
	},

	upload: {
		name: 'file',
		action: baseUrl + '/file/upload',
		headers: {
			Authorization: "Bearer " + localStorage.getItem("token")
		},
		beforeUpload(file) {
			let isJpgOrPng = file.type === 'image/jpeg' || file.type === 'image/png';
			if (!isJpgOrPng) {
				message.error('文件格式不为jpg或png');
			}
			return isJpgOrPng;
		}
	}
};

export default Axios;