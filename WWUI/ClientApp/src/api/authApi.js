import Axios from "../plugins/Axios";

const url = "admin/"

export default {
    getAuth(out) {
        Axios.get({
            url: url + "auth"
        }).then(res => {
            out(res)
        })
    },
    getRole(out) {
        Axios.get({
            url: url + "role"
        }).then(res => {
            out(res)
        })
    },
    getGrant(roleid, out) {
        Axios.get({
            url: url + "grant",
            params: {
                id: roleid
            }
        }).then(res => {
            out(res)
        })
    },
    changeGrant(data, out) {
        Axios.post({
            url: url + "grant",
            data
        }).then(res => {
            out(res)
        })
    },
    deleteRole(roleid, out) {
        Axios.delete({
            url: url + "role",
            params: {
                id: roleid
            }
        }).then(res => {
            out(res)
        })
    },
    addRole(name, out) {
        Axios.post({
            url: url + "role",
            params: {
                name: name
            }
        }).then(res => {
            out(res)
        })
    }
}