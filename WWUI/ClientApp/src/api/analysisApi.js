import Axios from "../plugins/Axios";

const url = "analysis/"

const analysisApi = {
    backstage(out) {
        Axios.get({
            url: url + "backstage"
        }).then(res => {
            out(res.data)
        })
    },
    typesdata(out) {
        Axios.get({
            url: url + "typesdata"
        }).then(res => {
            out(res.data)
        })
    }
}

export default analysisApi
