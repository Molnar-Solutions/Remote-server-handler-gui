import React from "react";
import {Card, CardContent, CardMedia, Chip, Divider, Grid} from "@mui/material";
import {Link} from "react-router-dom";
import Typography from "@mui/material/Typography";

export default function Thumbnail({title, url, bgImage}) {
    return <div style={{
        maxWidth: "500px",
        width: "100%",
        height: "auto",
    }}>
        <Link
            to={url}
            style={{textDecoration: "none"}}
        >
            <Card
                sx={{
                    borderRadius: 6,
                    boxShadow: 8,
                    backgroundColor: "rgb(0, 0, 0)",
                    height: "22vh",
                    position: "relative",
                    overflow: "hidden",
                    display: "flex",
                    flexDirection: "column",
                    justifyContent: "center",
                    transition: "transform 0.2s",
                    "&:hover": {
                        transform: "scale(1.05)",
                    },
                }}
            >
                {
                    bgImage && <CardMedia
                        component="img"
                        image={bgImage}
                        sx={{
                            height: "100%",
                            width: "100%",
                            objectFit: "cover",
                            filter: "blur(5px)",
                            position: "absolute",
                        }}
                    />
                }

                <CardContent
                    sx={{
                        zIndex: 1,
                        color: "#ffffff",
                        textAlign: "center",
                        backgroundColor: "rgba(0, 0, 0, 0.3)",
                    }}
                >
                    <Typography
                        variant="h4"
                        sx={{
                            overflow: "hidden",
                            textOverflow: "ellipsis",
                            width: "100%",
                            maxHeight: "100%",
                        }}
                    >
                        {title}
                    </Typography>
                </CardContent>
            </Card>
        </Link>
    </div>
}