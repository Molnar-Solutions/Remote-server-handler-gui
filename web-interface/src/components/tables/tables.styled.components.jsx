import {styled} from "@mui/material/styles";
import {Grid} from "@mui/material";

export const TableIcon = styled("b")(({ theme }) => ({
    backgroundColor: "blueviolet",
    padding: "0.5em",
    cursor: "pointer",
    color: "#fff",
    "&:hover": {
        border: "1px solid #fff"
    }
}));