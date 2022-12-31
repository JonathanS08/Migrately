import React from "react";
import { Link, useNavigate } from "react-router-dom";
import userService from "../../services/userService";
import migratelyLogo from "../../assets/images/migrately/migrately-logo.png";
import "./userstyles.css";
import { Formik, Form, Field, ErrorMessage } from "formik";
import { Card, Col, Row, Image, Button } from "react-bootstrap";
import * as validation from "../../schemas/userSchema";
import swal from "sweetalert2";
import logger from "sabio-debug";

const _logger = logger.extend("ResetPassword");

const ResetPassword = () => {
  const userEmailData = {
    email: "",
  };

  const navigate = useNavigate();

  const handleSubmit = (values) => {
    userService
      .resetPassword(values)
      .then(onResetSuccess)
      .catch(onResetFailure);
  };

  const onResetSuccess = () => {
    swal
      .fire(
        "Email Sent!",
        "An email has been sent to you to continue reseting your password.",
        "success"
      )
      .then(navigate("/"));
  };

  const onResetFailure = (response) => {
    _logger(response);
    swal.fire(
      "Error",
      "Reset Failed. Please make sure email is correct & try again.",
      "error"
    );
  };

  return (
    <React.Fragment>
      <div className="user-form-background">
        <Row className="align-items-center justify-content-center g-0 min-vh-100">
          <Col lg={5} md={5} className="py-8 py-xl-0">
            <Card>
              <Card.Body className="p-6">
                <div className="mb-4">
                  <Link to="/">
                    <Image src={migratelyLogo} className="mb-4" alt="" />
                  </Link>
                  <h1 className="mb-1 fw-bold">Reset Password</h1>
                  <span>Fill out the form to reset your password.</span>
                </div>

                <Formik
                  enableReinitialize={true}
                  initialValues={userEmailData}
                  onSubmit={handleSubmit}
                  validationSchema={validation.validationResetPassword}
                >
                  <Form>
                    <div className="mb-3">
                      <ErrorMessage
                        name="email"
                        className="user-form-error"
                        component="div"
                      />
                      <label htmlFor="email" className="user-form-label-text">
                        Email
                      </label>
                      <Field
                        type="email"
                        className="form-control"
                        name="email"
                      />
                    </div>
                    <Col lg={12} md={12} className="mb-0 d-grid gap-2">
                      <Button variant="primary" type="submit">
                        Send Reset Link
                      </Button>
                    </Col>
                  </Form>
                </Formik>
                <span>
                  <br />
                  Return to <Link to="/login">Sign in</Link>
                </span>
                <hr className="my-4" />
              </Card.Body>
            </Card>
          </Col>
        </Row>
      </div>
    </React.Fragment>
  );
};

export default ResetPassword;
