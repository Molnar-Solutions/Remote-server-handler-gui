import {Breadcrumbs, CircularProgress} from "@mui/material";
import {Link, useNavigate} from "react-router-dom";
import Typography from "@mui/material/Typography";
import DeleteMenu from "../../components/menu/delete-menu.component";
import AdvancedTable from "../../components/tables/advanced_table.component";
import {useEffect, useState} from "react";
import sendApiRequest, {NetworkMethods} from "../../lib/Network";
import {useSelector} from "react-redux";
import {getCurrentUser} from "../../store/reducers/user/user.reducer";
import toast from "react-hot-toast";
import moment from "moment";
import {styled} from "@mui/material/styles";
import {useDropzone} from "react-dropzone";
import ActionButton from "../../components/button/button.component";
import ArrowBackIcon from "@mui/icons-material/ArrowBack";

const MyDropZone = styled("div")(({ theme }) => ({
    width: "100%",
    maxHeight: "50px",
    minHeight: "50px",
    height: "100%",
    background: "#fff",
    display: "flex",
    justifyContent: "center",
    alignItems: "center",
    cursor: "pointer",
    backgroundBlendMode: "darken",
    color: '#111',
    fontWeight: "bold",
    border: "2px dashed #111"
}));

export default function FileManager()
{
    const navigate = useNavigate();
    const currentUser = useSelector(getCurrentUser);

    const [isLoading, setIsLoading] = useState(true);
    const [files, setFiles] = useState([]);
    const [open, setOpen] = useState(false);
    const [deletingName, setDeletingName] = useState("");

    useEffect(() => {
        collect();
    }, []);

    /* Get files from the server */
    const collect = async () => {
        try {
            setIsLoading(true);

            const response = await sendApiRequest({
                headers: {
                    'Authorization': 'Bearer ' + currentUser.token
                },
                method: NetworkMethods.POST,
                endpoint: `connector/list-files`,
                payload: {
                    email: currentUser.email
                }
            });

            if (!response.isSuccess) {
                toast.error(`Whoops! Something bad happened while fetching the data! Reason: ${response?.error ?? "No reason provided!"}`);
                return;
            }

            let apiResponse = response.data;

            setFiles(apiResponse.Data ?? apiResponse.data);
            setIsLoading(false);
        } catch (error) {
            toast.error(`Whoops! Something bad happened while fetching the data! Reason: ${error?.message ?? "No reason provided!"}`);
        }
    }

    /* Delete user file */
    const handleDelete = async () => {
        try {
            if (deletingName == "") return;

            const response = await sendApiRequest({
                headers: {
                    'Authorization': 'Bearer ' + currentUser.token
                },
                method: NetworkMethods.POST,
                endpoint: `connector/remove-file`,
                payload: {
                    email: currentUser.email,
                    fileName: deletingName ?? "-1"
                }
            });

            if (!response.isSuccess) {
                toast.error(`Error while deleting the file! Reason: ${response?.error ?? "No reason provided!"}`);
                return;
            }

            toast.success("File deleted successfully!");

            /* Update table contents */
            await collect();
        } catch (error) {
            toast.error(`Error while deleting the file! Reason: ${error?.message ?? "No reason provided!"}`);
        }
    }

    /* Download user file */
    const handleFileDownload = async (fileName) => {
        try {
            const response = await sendApiRequest({
                headers: {
                    'Authorization': 'Bearer ' + currentUser.token
                },
                method: NetworkMethods.GET,
                endpoint: `connector/download-file?fileName=${fileName}&email=${currentUser.email}`,
            });

            if (!response.isSuccess) {
                toast.error(`Error while downloading the file! Reason: ${response?.error ?? "No reason provided!"}`);
                return;
            }

            let apiResponse = response.data;
            let fileData = apiResponse.Data ?? apiResponse.data;

            /* The fileData is a file response, and this object contains two properties: type which is a string and a data which
            * is a byte array, I convert the data property to a file and I download it to local computer! */
            const blob = new Blob([new Uint8Array(fileData.data)], {type: fileData.type});
            const url = window.URL.createObjectURL(blob);
            const link = document.createElement("a");
            link.href = url;
            link.download = fileName || `server-handler-file-${new Date().getTime()}.txt`;
            document.body.appendChild(link);

            link.click();

            document.body.removeChild(link);
            window.URL.revokeObjectURL(url);

            toast.success("File downloaded successfully!");
        } catch (error) {
            toast.error(`Error while downloading the file! Reason: ${error?.message ?? "No reason provided!"}`);
        }
    }

    /* Handle dropzone actions */
    const onDrop = async (acceptedFiles) => {
        try {
            const file = acceptedFiles[0];

            return new Promise((resolve, reject) => {
                setIsLoading(true);
                const reader = new FileReader();

                reader.onload = () => {
                    const fileAsBase64 = reader.result;
                    const uploadingImage = {
                        base64: fileAsBase64,
                        deleted: false,
                        file: file,
                        modified: true,
                        isChecked: true,
                        url: "",
                    };

                    resolve(uploadingImage);
                };

                reader.onerror = reject;

                reader.readAsDataURL(file);
            }).then((convertedFile) => {
                const uploadedFile = convertedFile.file;

                if (!uploadedFile) {
                    toast.error('Whoops! Something bad happened while converting the file!');
                    return;
                }

                uploadFileToServer(uploadedFile);
            });
        } catch (error) {
            toast.error('Whoops! Something bad happened while converting the file!');
        }
    };

    /* Upload selected file to the server! */
    const uploadFileToServer = async (file) => {
        try {
            setIsLoading(true);

            const formData = new FormData();
            formData.append("fileContent", file);
            formData.append("email", currentUser.email);
            formData.append("fileName", currentUser.fileName);

            const response = await sendApiRequest({
                headers: {
                    Authorization: "Bearer " + currentUser.token,
                },
                method: NetworkMethods.POST,
                endpoint: `connector/upload-file`,
                payload: formData,
            });

            if (!response.isSuccess) {
                toast.error('Whoops! Something bad happened while uploading the file!');
                return;
            }

            setIsLoading(false);

            /* Refresh the files */
            await collect();
        } catch (e) {
            toast.error('Whoops! Something bad happened while uploading the file!');
        }
    }

    /* Step back to home page */
    const handleStepBack = () => navigate("/");

    const {getRootProps, getInputProps} = useDropzone({onDrop});

    if (isLoading) {
        return <div>
            <CircularProgress />
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
                to={"/filemanager"}
                target={"_self"}
                style={{
                    textDecoration: "none",
                    color: "inherit",
                    pointerEvents: "none"
                }}
            >
                <Typography variant="h5" textTransform={"lowercase"}>File manager</Typography>
            </Link>
        </Breadcrumbs>
        <ActionButton title={'Back to home page'} type={"STEP_BACK"} icon={<ArrowBackIcon/>}
                      action={handleStepBack}/>
        <br />
        <br />
        <Typography variant='h3'>File manager</Typography>
        <br/>
        <br/>
        <MyDropZone {...getRootProps()} style={{width: "100%"}}>
            <input {...getInputProps()} />
            <Typography variant="body1">
                Drop files here or click to upload
            </Typography>
        </MyDropZone>
        <br/>
        <br/>
        <AdvancedTable
            columns={[
                "File name",
                "Owner",
                "Size in bytes",
                "Created at",
                "Privilege"
            ]}
            rows={
                files.map(file => ({
                    fileName: file.fileName,
                    owner: file.owner,
                    sizeInBytes: file.sizeInBytes,
                    createdAt: moment(file.createdAt).format("DD/MM/YYYY HH:mm:ss"),
                    privilege: file.privilege
                }))
            }
            properties={{
                page: 1,
                pages: 1,
                rowsPerPage: 30,
                disableSee: true,
                disableEdit: true,
                disableDelete: false,
                disableDownload: false
            }}
            rowActions={
                {
                    deleteAction: (row) => {
                        setDeletingName(row.fileName);
                        setOpen(true);
                    },
                    editAction: () => {
                    },
                    seeAction: (row) => {},
                    downloadAction: (row) => handleFileDownload(row.fileName)
                }
            }
            pageChange={() => {}}
        />

        <DeleteMenu open={open} handleClose={() => setOpen(!open)} handleDelete={handleDelete}/>
    </div>
}