import {Link, useNavigate} from "react-router-dom";
import Typography from "@mui/material/Typography";
import {Breadcrumbs, CircularProgress, Stack, TextField} from "@mui/material";
import {useEffect, useState} from "react";
import AdvancedTable from "../../components/tables/advanced_table.component";
import sendApiRequest, {NetworkMethods} from "../../lib/Network";
import toast from "react-hot-toast";
import {useSelector} from "react-redux";
import {getCurrentUser} from "../../store/reducers/user/user.reducer";
import {styled} from "@mui/material/styles";
import ActionButton from "../../components/button/button.component";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";

const InfoWrapper = styled(Stack)(({theme}) => ({
    flexDirection: "row",
    flexWrap: "wrap",
    justifyContent: "flex-start",
    alignItems: "center",
    marginBottom: "0.5rem"
}))

const KeyField = styled('p')(({theme}) => ({
    [theme.breakpoints.down("lg")]: {
        ...theme.typography.h6
    },
    [theme.breakpoints.up("lg")]: {
        ...theme.typography.h5
    }
}))


const ValueField = styled(TextField)(({theme}) => ({
    maxWidth: "500px",
    width: "100%",
    background: "#fff",
    marginLeft: "auto",
}))

export default function SystemHealth() {
    const navigate = useNavigate();
    const currentUser = useSelector(getCurrentUser);

    const [isLoading, setIsLoading] = useState(true);
    const [systemHealth, setSystemHealth] = useState({});

    useEffect(() => {
        collect();
    }, []);

    /* Get info from the server */
    const collect = async () => {
        try {
            setIsLoading(true);

            const response = await sendApiRequest({
                headers: {
                    'Authorization': 'Bearer ' + currentUser.token
                },
                method: NetworkMethods.POST,
                endpoint: `connector/system-health`,
                payload: {
                    email: currentUser.email
                }
            });

            if (!response.isSuccess) {
                toast.error(`Whoops! Something bad happened while fetching the data! Reason: ${response?.error ?? "No reason provided!"}`);
                return;
            }

            let apiResponse = response.data;

            setSystemHealth(apiResponse.Data ?? apiResponse.data);
            setIsLoading(false);
        } catch (error) {
            toast.error(`Whoops! Something bad happened while fetching the data! Reason: ${error?.message ?? "No reason provided!"}`);
        }
    }

    /* Step back to home page */
    const handleStepBack = () => navigate("/");

    if (isLoading) {
        return <div>
            <CircularProgress/>
        </div>
    }

    return <div>
        <Breadcrumbs aria-label="breadcrumb">
            <Link
                to={"/"}
                target={"_self"}
                style={{
                    textDecoration: "none",
                    color: "inherit",
                    pointerEvents: "none"
                }}
            >
                <Typography variant="h5" textTransform={"lowercase"}>Home page</Typography>
            </Link>
            <Link
                to={"/systemhealth"}
                target={"_self"}
                style={{
                    textDecoration: "none",
                    color: "inherit",
                    pointerEvents: "none"
                }}
            >
                <Typography variant="h5" textTransform={"lowercase"}>System health</Typography>
            </Link>
        </Breadcrumbs>
        <ActionButton title={'Back to home page'} type={"STEP_BACK"} icon={<ArrowBackIcon/>}
                      action={handleStepBack}/>
        <br />
        <br />
        <Typography variant='h3'>System health</Typography>
        <br/>
        <br/>

        <InfoWrapper flexDirection={"row"} flexWrap={"wrap"} justifyContent={"space-evenly"} alignItems={"center"}>
            <KeyField variant='h4'>OS Type (GB): </KeyField>
            <ValueField value={systemHealth?.osType ?? ""} disabled/>
        </InfoWrapper>

        <InfoWrapper flexDirection={"row"} flexWrap={"wrap"} justifyContent={"space-evenly"} alignItems={"center"}>
            <KeyField variant='h4'>Architecture: </KeyField>
            <ValueField value={systemHealth?.architecture ?? ""} disabled/>
        </InfoWrapper>

        <InfoWrapper flexDirection={"row"} flexWrap={"wrap"} justifyContent={"space-evenly"} alignItems={"center"}>
            <KeyField variant='h4'>CPU Usage(%): </KeyField>
            <ValueField value={(systemHealth?.cpuUsage * 100) ?? ""} disabled/>
        </InfoWrapper>

        <InfoWrapper flexDirection={"row"} flexWrap={"wrap"} justifyContent={"space-evenly"} alignItems={"center"}>
            <KeyField variant='h4'>Available memory(GB): </KeyField>
            <ValueField value={systemHealth?.availableMemory ?? ""} disabled/>
        </InfoWrapper>

        <InfoWrapper flexDirection={"row"} flexWrap={"wrap"} justifyContent={"space-evenly"} alignItems={"center"}>
            <KeyField variant='h4'>Total memory(GB): </KeyField>
            <ValueField value={systemHealth?.totalMemory ?? ""} disabled/>
        </InfoWrapper>

        <InfoWrapper flexDirection={"row"} flexWrap={"wrap"} justifyContent={"space-evenly"} alignItems={"center"}>
            <KeyField variant='h4'>Available storage (sum of all partition)(GB): </KeyField>
            <ValueField value={systemHealth?.availableStorage ?? ""} disabled/>
        </InfoWrapper>
        <br/>
        <br/>
        <AdvancedTable
            columns={[
                "Log information"
            ]}
            rows={
                systemHealth?.logs && systemHealth.logs.map(log => ({
                    info: log
                }))
            }
            properties={{
                page: 1,
                pages: 1,
                rowsPerPage: 30,
                disableSee: true,
                disableEdit: true,
                disableDelete: true,
                disableDownload: true
            }}
            rowActions={
                {
                    deleteAction: (row) => {
                    },
                    editAction: () => {
                    },
                    seeAction: (row) => {
                    },
                    downloadAction: (row) => {
                    }
                }
            }
            pageChange={() => {
            }}
        />
    </div>
}