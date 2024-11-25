import {Button, Stack, TableHead, Tooltip, useTheme} from "@mui/material";

import "./table.css"

import EditIcon from '@mui/icons-material/Edit';
import RemoveRedEyeIcon from '@mui/icons-material/RemoveRedEye';
import DeleteIcon from '@mui/icons-material/Delete';
import DownloadIcon from '@mui/icons-material/Download';

import {TableIcon} from "./tables.styled.components";
import IconButton from "@mui/material/IconButton";
import ArrowForwardIcon from '@mui/icons-material/ArrowForward';
import ArrowBackIcon from '@mui/icons-material/ArrowBack';
import Typography from "@mui/material/Typography";

const generateEmptyRows = (times) => {
    let rows = [];

    for (let i = 0; i < times; i++) {
        rows.push(1);
    }

    return rows;
}

export default function AdvancedTable({
                                             columns,
                                             rows,
                                             pageChange,
                                             properties: {
                                                 rowsPerPage,
                                                 page,
                                                 pages,
                                                 disableDelete,
                                                 disableSee,
                                                 disableEdit,
                                                 disableDownload
                                             },
                                             rowActions: {
                                                 deleteAction,
                                                 editAction,
                                                 seeAction,
                                                 downloadAction
                                             }
                                         }) {
    return (
        <div style={{overflowX: "auto"}}>
            <table style={{width: "100%", minHeight: "500px"}} className={"customTable"}>
                <tr style={{height: "50px"}}>
                    {
                        !(disableDelete && disableEdit && disableSee && disableDownload) && <th>Functions</th>
                    }
                    {
                        columns.map((column, i) => (
                            <th key={column + " " + i}>{column}</th>
                        ))
                    }
                </tr>
                {
                    rows && rows
                        .slice(0, rowsPerPage)
                        .map((row, i) => {
                            return (
                                <tr key={i}>
                                    {
                                        (!disableDelete || !disableEdit || !disableSee) && (<td>
                                                <Stack direction="row" spacing={1} justifyContent={"center"}>
                                                    {
                                                        !disableEdit && (
                                                            <TableIcon onClick={() => editAction(row)}>
                                                                <Tooltip title={"Edit"}>
                                                                    <EditIcon/>
                                                                </Tooltip>
                                                            </TableIcon>
                                                        )
                                                    }
                                                    {
                                                        !disableSee && <TableIcon onClick={() => seeAction(row)}>
                                                            <Tooltip title={"See"}>
                                                                <RemoveRedEyeIcon/>
                                                            </Tooltip>
                                                        </TableIcon>
                                                    }
                                                    {
                                                        !disableDownload && <TableIcon onClick={() => downloadAction(row)}>
                                                            <Tooltip title={"Download"}>
                                                                <DownloadIcon/>
                                                            </Tooltip>
                                                        </TableIcon>
                                                    }
                                                    {
                                                        !disableDelete &&
                                                        <TableIcon sx={{color: "#fff", backgroundColor: "red"}}
                                                                   onClick={() => deleteAction(row)}>
                                                            <Tooltip title={"Delete"}>
                                                                <DeleteIcon/>
                                                            </Tooltip>
                                                        </TableIcon>
                                                    }
                                                </Stack>
                                            </td>
                                        )
                                    }
                                    {
                                        Object.values(row).map((value, i) => {
                                            return (
                                                <td key={i}>
                                                    {value}
                                                </td>
                                            )
                                        })
                                    }
                                </tr>
                            )
                        })
                }

                {
                    rows && rows?.length && rows.length < rowsPerPage && generateEmptyRows(rowsPerPage - rows.length).map((row, i) => (
                        <tr key={row + " " + i}></tr>
                    ))
                }
            </table>
            <div className="customTableActions">
                <Stack flexDirection="row" flexWrap={"nowrap"} justifyContent="space-between" alignItems="center">
                    <Typography variant="body1" style={{color: "#fff"}}>Page: {page}</Typography>
                    <div>
                        <IconButton onClick={() => pageChange(page - 1)} style={{color: (page == 1) ? "#ccc" : "#fff"}}
                                    disabled={(page == 1)}>
                            <ArrowBackIcon/>
                        </IconButton>
                        <IconButton onClick={() => pageChange(page + 1)}
                                    style={{color: ((page == pages)) ? "#ccc" : "#fff"}} disabled={(page == pages)}>
                            <ArrowForwardIcon/>
                        </IconButton>
                    </div>
                </Stack>
            </div>
        </div>
    )
}