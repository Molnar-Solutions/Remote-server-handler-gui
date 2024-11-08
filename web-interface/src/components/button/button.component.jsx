import {Button} from "@mui/material";
import {styled} from "@mui/material/styles";

const MyButton = styled(Button)(({theme}) => ({
    [theme.breakpoints.down("md")]: {
        ...theme.typography.body1,
    },
    [theme.breakpoints.up("md")]: {
        ...theme.typography.h6,
    },
    "&:hover": {
        backgroundColor: "#012041",
        color: "#fff"
    },
}))

function returnStyles(type) {
    const styles = {
        color: "#111",
        backgroundColor: "initial",
        border: "none",
        borderRadius: "5px",
        textTransform: "none",
        fontSize: "1rem",
        padding: "5px 10px",
        cursor: "pointer",
        maxWidth: "350px",
    }

    switch (type) {
        case "SAVE":
            styles.backgroundColor = "#202124";
            styles.color = "#fff";
            styles.border = "none";
            break;
        case "ADD":
            styles.backgroundColor = "#202124";
            styles.color = "#fff";
            styles.border = "none";
            break;
        case "SIGN_IN":
            styles.backgroundColor = "#DB614B";
            styles.color = "#fff";
            styles.border = "none";
            break;
        case "SEARCH":
            styles.backgroundColor = "#DB614B";
            styles.color = "#fff";
            styles.border = "none";
            break;
        case "REMOVE":
            styles.backgroundColor = "red";
            styles.color = "#fff";
            styles.border = "none";
            break;
        case "CANCEL":
            styles.backgroundColor = "#F9F9F9";
            styles.color = "#111";
            styles.border = "none";
            break;
        case "BLUE":
            styles.backgroundColor = "#012041";
            styles.color = "#fff";
            styles.border = "none";
            break;
        case "GRAY":
            styles.backgroundColor = "#ccc";
            styles.color = "#111";
            styles.border = "none";
            break;
        case "STEP_BACK":
            styles.backgroundColor = "none";
            styles.border = "2px solid gray";
            styles.borderRadius = "4px";
            styles.color = "#111";
            styles.border = "none";
            break;
    }

    return styles;
}

const baseStyles = {
    margin: "5px 0"
}

export default function ActionButton({ title, type, action, disabled, icon }) {
    return <MyButton sx={{ ...returnStyles(type), ...baseStyles }} onClick={action} disabled={disabled} startIcon={icon} >{title}</MyButton>
}