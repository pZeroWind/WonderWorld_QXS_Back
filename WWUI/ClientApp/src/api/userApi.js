import Axios from "../plugins/Axios";

const url = "/user"

const UserApi = {
    login(data, out) {
        Axios.post({
            url: "/admin/login",
            data: data
        }).then(res => {
            out(res)
        })
    },
    get(data, out) {
        Axios.get({
            url: url + "/" + data
        }).then(res => {
            out(res.data)
        })
    },
    getGrant(out) {
        Axios.get({
            url: "/admin/getGrant"
        }).then(res => {
            out(res.data)
        })
    },
    getAll(params, out) {
        Axios.get({
            url: url + "/get",
            params
        }).then(res => {
            out(res)
        })
    },
    changeRole(params, out) {
        Axios.post({
            url: url + "/ChangeRole",
            params
        }).the(res => {
            out(res)
        })
    }
}

export default UserApi