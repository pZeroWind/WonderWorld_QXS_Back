import Axios from "../plugins/Axios";
const url = "report/"
export default {
    get(page, out) {
        Axios.get({
            url,
            params: {
                page: page
            }
        }).then(res => {
            out(res)
        })
    },
    complete(id, out) {
        Axios.get({
            url,
            params: {
                id: id
            }
        }).then(res => {
            out(res)
        })
    }
}